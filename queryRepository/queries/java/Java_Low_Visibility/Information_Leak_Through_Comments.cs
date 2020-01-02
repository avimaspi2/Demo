CxList objectCreate = Find_Object_Create();
CxList strings = Find_Strings();

//Find passwords leaked through comments in HTML
CxList htmlOutput = Find_Web_Outputs();
htmlOutput.Add(Find_Html_Outputs());

CxList htmlRemarks = strings.FindByName("*<!--*");
htmlRemarks.Add(strings.FindByName("*-->*"));

htmlOutput = htmlOutput.DataInfluencedBy(htmlRemarks);
CxList pass = Find_Password_Strings();
result = htmlOutput.DataInfluencedBy(pass);

//Find comments with sensible information generated through JAVA libraries
CxList suspectStrings = strings.FindByShortNames(
	new List<string> {"FIX*","TO DO", "TODO", "WIP", "admin", "pass*", "*pw*"}, false);
CxList suspectObjects = objectCreate.FindByShortNames(new List<string> {"Comment"}, false);
result.Add(suspectObjects.DataInfluencedBy(suspectStrings));

//Find information leacks in HTML comments
string pattern = @"(\b)(FIX(\s*ME)?|(TO\s*DO)|WIP|DELETE|UPDATE|REFACTOR)(\b)";
List < string > extensions = new List<string>
	{"*.cshtml", "*.xhtml", "*.jsf", "*.jsp", "*.jspf", "*.htm", "*.html", "*.phtml", "*.phtm", "*.rhtml", "*.vm"};
CxList suspectComments = All.FindByRegexExt(pattern, extensions, true, CxList.CxRegexOptions.SearchOnlyInComments, RegexOptions.IgnoreCase);
result.Add(suspectComments);