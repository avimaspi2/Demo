// Cut_And_Paste_Leakage (Privacy_Violation)
// //////////////////////-
// This query returns personal data which is presented on the screen,
// and enables cutting that info and pasting it outside the application.

// The query checks for three sanitizing options of info:
//   1. Marking the field as a secureTextEntry through the code.
//   2. Overriding options that disable the buffer for UITextView objects.
//   3. Manually clearing the pasteboard before entering background.

CxList personalInfo = Find_Personal_Info();
CxList methods = Find_Methods();

// Find the functions that run right before entering background.
CxList enterBackground = Find_EnterBackground_Statements();

List<string> methodsNames = new List<string>{"setString", "string"};
CxList setString = All.FindByShortNames(methodsNames);

CxList setStringMethods = setString.FindByMemberAccess("UIPasteboard.setString:");	// [pasteboard setString:@""]
setStringMethods.Add(setString.FindByMemberAccess("generalPasteboard.setString:")); // 	[[UIPasteboard generalPasteboard] setString:@""]
CxList setStringAssign = setString.FindByMemberAccess("UIPasteboard.string"); // pasteboard.strings = @""
setStringAssign.Add(setString.FindByMemberAccess("generalPasteboard.string")); // 	[UIPasteboard generalPasteboard].string = @""
CxList setStringBackground = setStringMethods.GetByAncs(enterBackground);

CxList emptyStringsAll = Find_Empty_Strings();

CxList stringClear = Find_Null_Literals();
stringClear.Add(emptyStringsAll);
stringClear.Add(Find_Strings());

CxList pasteboardCleared = setStringBackground.FindByParameters(stringClear);	// [pasteboard setString:@""]
pasteboardCleared.Add(setStringAssign * stringClear.GetFathers());	// UIPasteboard *pasteboard.strings = @"" assignment at declararion statement
CxList setStringAssigment = setStringAssign.FindByAssignmentSide(CxList.AssignmentSide.Left).GetFathers(); // .strings = @""
pasteboardCleared.Add(setStringAssign.FindByFathers(setStringAssigment * stringClear.GetFathers()));	// pasteboard.strings = @"" assignment not at declararion statement

CxList clearedClasses = pasteboardCleared.GetAncOfType(typeof(ClassDecl));
clearedClasses.Add(Find_Interfaces(clearedClasses));

foreach (CxList classClear in clearedClasses)
{
	personalInfo -= personalInfo.FindAllReferences(personalInfo.GetByAncs(classClear));
}

CxList inputs = Find_Inputs();
inputs.Add(Find_DB_Out());

personalInfo = personalInfo.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly) + 
	personalInfo * inputs;

// look for methods "canPerformAction:withSender:" that address the cut: and copy: actions, and returns no on certain conditions:
CxList sanitizeMethods = methods.FindByShortName("canPerformAction:withSender:");

//Find classes that inherit from UITextView and override options that disable the buffer
CxList interfaces = Find_InterfaceDecl();
CxList classes = Find_Classes();

CxList declarators = All.NewCxList();
declarators.Add(classes);
declarators.Add(interfaces);

CxList UIFields = declarators.InheritsFrom("UITextView");
UIFields.Add(declarators.InheritsFrom("UITextField"));

UIFields *= classes;

CxList sanitizeClasses = UIFields * sanitizeMethods.GetAncOfType(typeof(ClassDecl));
// personalInfo -= personalInfo.FindByTypes(sanitizeClasses); // changed code for backward compatibility
foreach (CxList sanitized in sanitizeClasses)
	personalInfo -= personalInfo.FindByType(sanitized);

if (personalInfo.Count > 0) // if the application is not entirely secured
{
	//Remove immutable strings from personalInfo
	CxList immutableStrings = personalInfo.FindByType(typeof(StringLiteral));
	immutableStrings -= immutableStrings.FindByRegex(@"[%?]");
	personalInfo -= immutableStrings;

	//Remove assignments of empty strings
	CxList emptyStrings = emptyStringsAll.FindByFathers(personalInfo.GetFathers());
	personalInfo -= personalInfo.FindByFathers(emptyStrings.GetFathers());

	//Remove secure fields from personalInfo
	CxList secureUIFields = Find_Secured_UI_Widgets(personalInfo);
	personalInfo -= personalInfo.FindAllReferences(secureUIFields);

	CxList uiWWidgets = Find_UI_Widgets();
	
	string[] uiWWidgetsTypes = new string[]{"UITextField","UITextView"};
	
	CxList definitions = uiWWidgets.FindByTypes(uiWWidgetsTypes);
	
	CxList output = Find_Interactive_Outputs_User();
	output -= output.FindByType(@"UILabel");
	result.Add(personalInfo.DataInfluencingOn(output));

	definitions = definitions.FindDefinition(personalInfo - output);
	result.Add(definitions);
	
}
else 		// The entire app is secured by clearing the pasteboard when entering background or by blocking cut: and copy: actions.
{
	result = All.NewCxList();
}