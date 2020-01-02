//1st part - Typescript
CxList methods = Find_Methods();
//Find variables assigned by window.open();
//e.g var window = window.open();
CxList windowOpen = methods.FindByMemberAccess("window.open");

CxList stringLiterals = Find_String_Literal();
//The only vulnerable cases are: if the second parameter of window.open is an empty string or the string "_blank"
CxList targetBlankArguments = stringLiterals.FindByShortName("_blank");
targetBlankArguments.Add(stringLiterals.FindByShortName(""));

//When there is no second parameter at all, the default is blank. 
CxList windowOpenSecondParameters = All.GetParameters(windowOpen, 1);
CxList windowOpenWithoutParameters = windowOpen - windowOpen.FindByParameters(windowOpenSecondParameters);
//We are left with all the window.open methods with at least 2 parameters.
windowOpen -= windowOpenWithoutParameters;

//The list of window.open parameters that are either "_blank" or "".
CxList unsafeDirectArguments = windowOpenSecondParameters * targetBlankArguments;

/*	In case the 2nd parameter isn't a StringLiteral, check if "_blank" 
	or ""(empty) string flow to it. 
  	We are left with the unsafe window.open methods.	
*/
CxList targetUnsafeArg = windowOpenSecondParameters.DataInfluencedBy(targetBlankArguments);
targetUnsafeArg.Add(unsafeDirectArguments);

windowOpen = windowOpen.FindByParameters(targetUnsafeArg);
//Add those without second parameter to the unsafe list.
windowOpen.Add(windowOpenWithoutParameters);

CxList leftSide = All.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList varOfWindowOpen = windowOpen.GetAssignee(leftSide);

CxList inputs = Find_Inputs();
CxList conditions = Find_Conditions();
CxList interestingConditions = conditions.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList interestingStatements = interestingConditions.GetAncOfType(typeof(IfStmt));

//sanitization 1st part
CxList nullLiteral = Find_NullLiteral();
CxList windowOpener = All.FindByMemberAccess("*.opener");
CxList windowOpenerNull = nullLiteral.GetAssignee(windowOpener);
//remove window.opener = null if it is influenced by the user input
windowOpenerNull -= windowOpenerNull.GetByAncs(windowOpenerNull.GetAncOfType(typeof(IfStmt)) * interestingStatements);

CxList referencesWindowOpenerNull = All.FindAllReferences(windowOpenerNull.GetTargetOfMembers());
//Remove variables that has the opener = null
//e.g window.opener = null;
varOfWindowOpen -= referencesWindowOpenerNull;
result.Add(varOfWindowOpen);

//2nd part - HTML
string pattern = @"<a\s+((?<anch><)|(?<-anch>>)|[^<>])+>(?(anch)(?!))";
List < string > extensions = new List<string>{"*.inc", "*.asax", "*.ascx", "*.aspx", "*.master"
		, "*.cshtml", "*.jsf", "*.jsp", "*.jspf", "*.xhtml", "*.vm"
		, "*.handlebars", "*.hbs", "*.htm", "*.html", "*.jade", "*.pug"
		, "*.apexp", "*.page", "*.component", "*.php", "*.php3", "*.php4"
		, "*.php5", "*.phtml", "*.apex", "*.asp", "*.phtm", "*.twig"
		, "*.erb", "*.rhtml", "*.rjs", "*.xsjs", "*.xsjslib"
		, "*.pl", "*.cgi", "*.pm", "*.py", "*.gtl", "*.js", "*.xsaccess", "*.xsapp"};
CxList matches = All.FindByRegexExt(pattern, extensions, false, CxList.CxRegexOptions.None, RegexOptions.None);
CxList htmlResults = All.NewCxList();
foreach (CxList m in matches){
	Comment match = m.TryGetCSharpGraph<Comment>();
	string matchText = match.FullName;
	//Find this pattern in anchors:
	//e.g <a href="http://www.w3schools.com" target="_blank">
	string targetPattern = @"target\s*=\s*('|""|\\"")_?blank('|""|\\"")";
	Regex regexTarget = new Regex(targetPattern);
	Match matchTarget = regexTarget.Match(matchText);
	if(matchTarget.Success){
		string relPattern = @"rel\s*=\s*('|""|\\"")noopener|noopener\s+noreferrer|noreferrer\s+noopener('|""|\\"")";
		Regex regexRel = new Regex(relPattern);
		Match matchRel = regexRel.Match(matchText);
		//Do not add anchors if they have rel = "noopener" or rel = "noopener noreferrer"
		//e.g  <a href="http://www.w3schools.com" target="_blank" rel = "noopener noreferrer">
		//e.g  <a href="http://www.w3schools.com" target="_blank" rel = "noopener">
		if(!matchRel.Success){
			htmlResults.Add(m);
		}
	}
}

//find setAttribute methods (it can sanitize the html)
CxList setAttribute = methods.FindByShortName("setAttribute");
List <string> relAttributes = new List<string>{"\"noopener\"", "\"noopener noreferrer\""};
//find first parameter of setAttribute
CxList setAttributeFirstParam = stringLiterals.GetParameters(setAttribute, 0).FindByShortName("\"rel\"");
//setAttribute will sanitize with a combination of "rel" with "noopener" or "noopener noreferrer"
CxList sanitizers = setAttribute.FindByParameters(setAttributeFirstParam);
sanitizers = setAttribute.FindByParameters(stringLiterals.GetParameters(sanitizers, 1).FindByShortNames(relAttributes));
//Unsafe_Use_of_Target_Blank
//Individual sanitizer
//Find methods that can use anchor tag
CxList elementsByTagName = sanitizers.GetTargetOfMembers().GetTargetOfMembers().FindByShortName("getElementsByTagName");
CxList interstingParam = stringLiterals.FindByShortName("a");
//Find methods that use anchor tag
CxList elementsByTagNameMethod = elementsByTagName.FindByParameters(interstingParam);
//Find only methods that are arrays
CxList individualSanitizers = elementsByTagNameMethod.GetMembersOfTarget() - sanitizers;

CxList toRemove = All.NewCxList();

foreach(CxList individualSanitizer in individualSanitizers){
	CSharpGraph gIndex = individualSanitizer.TryGetCSharpGraph<CSharpGraph>();
	int index = 0;
	//Try to get the integer that access to the array position
	bool parsed = int.TryParse(gIndex.ShortName, out index);
	if(parsed){
		//Get file name of node
		string fileName = gIndex.LinePragma.FileName;
		//Find results that are from the sanitization file
		CxList resultsOfFile = htmlResults.FindByFileName(fileName);
		int counter = 0;
		foreach(CxList res in resultsOfFile){
			//if current result is the same that the index, then the result is sanitized
			//this will assume that the resultsOfFile came by the order of line and column due to regex behavior
			if(counter == index){
				toRemove.Add(res);
				break;
			}
			counter++;
		}
	}
}

//This will sanitize all anchors in file
CxList fileSanitizers = sanitizers - individualSanitizers.GetMembersOfTarget();

List<string> sanitizedFiles = new List<string>();
//Get files that use setAttribute sanitized
foreach(CxList sanitizer in fileSanitizers){
	CSharpGraph fName = sanitizer.TryGetCSharpGraph<CSharpGraph>();
	sanitizedFiles.Add(fName.LinePragma.FileName);
}

if(sanitizedFiles.Count > 0){
	//If the the result is in santized file then it should be removed
	foreach(CxList htmlResult in htmlResults){
		CSharpGraph fName = htmlResult.TryGetCSharpGraph<CSharpGraph>();
		if(sanitizedFiles.Contains(fName.LinePragma.FileName)){
			toRemove.Add(htmlResult);
		}
	}
}

htmlResults -= toRemove;

//Get the React query results and remove the repeated results found in this query
CxList reactResults = React_Find_TargetBlank();
foreach (CxList reactResult in reactResults)
{
	LinePragma lp = reactResult.TryGetCSharpGraph<CSharpGraph>().LinePragma;
	CxList htmlRes = htmlResults.FindByPosition(lp.FileName, lp.Line);
	if (htmlRes.Count > 0)
	{
		htmlResults -= htmlRes;
	}
	result.Add(reactResult);
}

result.Add(htmlResults);