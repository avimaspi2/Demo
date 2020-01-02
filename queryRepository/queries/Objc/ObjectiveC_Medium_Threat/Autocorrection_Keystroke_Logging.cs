// Keystroke Logging of sensitive information(as part of Privacy_Violation) into the IOS's dictionary used for autocorrection
// This query find UITextView and UITextField that does not have autocorrection turned off and are not defined as secured.
CxList sensitive = Find_UI_Widgets_With_Sensitive_Data();

string[] types = {"UITextView", "UITextField"};

CxList sensitiveTypes = sensitive.FindByTypes(types);

CxList AutoCorrectRemoved = Find_Autocorrection_Off(sensitive);
result = All.FindDefinition(sensitiveTypes - sensitiveTypes.FindByShortName(AutoCorrectRemoved));