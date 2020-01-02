// Side Channel Data Leakage (a part of Privacy_Violation)
// //////////////////////-
// The following cases will be classified as Privacy Violation
// 
// 1) Personal information kept in log file 
// 2) Keystroke logging of sensitive information
// 3) Enabled iOS screenshot capture for sensitive information
//

CxList sensitive = Find_UI_Widgets_With_Sensitive_Data();

// 1) Personal information kept in log file
CxList personalInfo = Find_Personal_Info(); 
CxList logOutput = Find_Log_Outputs();
CxList sanitize = Find_General_Sanitize();

CxList inputs = Find_Inputs();
inputs.Add(Find_DB_Out());

CxList personalInfoInputs = personalInfo * inputs;

personalInfo = personalInfo.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
personalInfo.Add(personalInfoInputs);

CxList personalInfoResult = personalInfo.InfluencingOnAndNotSanitized(logOutput, sanitize);

CxList interactiveOutputsLogOutput = Find_Interactive_Outputs();
interactiveOutputsLogOutput.Add(logOutput);
	
result.Add(personalInfoResult.InfluencingOnAndNotSanitized(interactiveOutputsLogOutput, sanitize));
result.Add(personalInfoResult.InfluencedByAndNotSanitized(Find_Interactive_Inputs_User(), sanitize));	

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

// 2) Keystroke logging of sensitive information - add the UI objects that are not defined as secured
//    and the autocorrection is not turned off.
// Keystroke logging is rellevant only to UITextField and UITextView objects
CxList sensitiveText = sensitive.FindByTypes(new string[]{"UITextField","UITextView"});

CxList AutoCorrectRemoved = Find_Autocorrection_Off(sensitiveText);
result.Add(All.FindDefinition(sensitiveText - sensitiveText.FindByShortName(AutoCorrectRemoved)));

// 3) Enabled IOS screenshot capture for sensitive information - add the UI objects that are not hidden defined as secured
//    when the application goes to background, and so their content can be captured by the app-switcher
result -= All.FindDefinition(Find_Screen_Caching(sensitive));