// param[0] is a sensitive UI object, or Find_UI_Widgets_With_Sensitive_Data() by default
// The query will address only UITextField and UITextView objects and return the references which remove the autocorrection
// Example:
// 1. UITextField* x = [[UITextField alloc] init];
// 2. [x autocorrectionType:UITextAutocorrectionTypeNo];
// the query will return x from line 2 only.
CxList sensitive;
if (param.Length == 1)
	sensitive = param[0] as CxList;
else
	sensitive = Find_UI_Widgets_With_Sensitive_Data();
	
// Find text whith autocorrection turned off
CxList autoCorrectMethods = All.FindByShortName("*autocorrectionType*");
CxList autoCorrect = autoCorrectMethods.FindByType(typeof(MemberAccess));
autoCorrect.Add(autoCorrectMethods.FindByType(typeof(MethodInvokeExpr)));

CxList removedAutoCorrect = autoCorrect.FindByFathers(All.FindByShortName("UITextAutocorrectionTypeNo").GetFathers());
removedAutoCorrect.Add(autoCorrect.FindByFathers(All.FindByMemberAccess("UITextAutocorrectionType.No").GetFathers()));
removedAutoCorrect.Add(autoCorrect.FindByFathers(All.FindByMemberAccess("UITextAutocorrectionType.no").GetFathers()));
removedAutoCorrect.Add(autoCorrect.FindByFathers(All.FindByMemberAccess(".no").GetFathers()));

string[] typesNames = new string[]{"UITextField","UITextView"};
CxList personalInfoUI = sensitive.FindByTypes(typesNames);

result = personalInfoUI * All.FindDefinition(removedAutoCorrect.GetTargetOfMembers());
result.Add(Find_Secured_UI_Widgets(personalInfoUI)); // secured UI objects have Autocorrection turned off