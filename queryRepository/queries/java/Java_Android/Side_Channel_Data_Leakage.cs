// Query Side_Channel_Data_Leakage (Privacy_Violation)
// -------------------------------------------------

CxList outputs = Find_Outputs();
CxList sanitize = Find_Encrypt();

//Find Personal Info
CxList passwordIdentifier = All.FindByShortName("*password*", false).GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("findViewById");
CxList editText = All.FindByType("EditText");
CxList passwordView = editText.DataInfluencedBy(passwordIdentifier) * editText;

CxList personal_info = Find_Personal_Info();
personal_info.Add(All.FindAllReferences(passwordView));

CxList pwdAndPersonalInfo = Find_Passwords();
pwdAndPersonalInfo.Add(personal_info);

CxList inputs = Find_Inputs();
inputs.Add(Find_DB_Out());

//Personal info and passwords that are inputs
CxList dataToLeak = pwdAndPersonalInfo * inputs;
//Add passwords and personal info influenced by inputs
dataToLeak.Add(pwdAndPersonalInfo.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//Outputs influenced by inputs personal info and passwords
CxList infOn = dataToLeak.InfluencingOnAndNotSanitized(outputs, sanitize);

//Remove duplicate smaller flows
result = infOn.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);