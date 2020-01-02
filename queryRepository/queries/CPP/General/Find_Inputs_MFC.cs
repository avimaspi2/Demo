CxList methods = Find_Methods();
CxList methodDecl = Find_Method_Declarations();

// MFC DDX: DDX_Control, DDX_Text, etc...
CxList outParams = All.GetParameters(methods.FindByShortName("DDX_*"), 2);
// preliminary - list to be extended/tested
outParams.Add(All.GetParameters(methods.FindByMemberAccess("CEdit.Get*"), 1));
outParams.Add(All.GetParameters(methods.FindByMemberAccess("CHtmlEditCtrl.GetDHtmlDocument"), 0));
outParams.Add(All.GetParameters(methods.FindByMemberAccess("CListBox.GetText"), 1));
outParams.Add(All.GetParameters(methods.FindByMemberAccess("CListCtrl.GetItemText"), 2));
outParams.Add(All.GetParameters(methods.FindByMemberAccess("CRichEditCtrl.Get*"), 1));
outParams.Add(All.GetParameters(methods.FindByMemberAccess("CComboBox.Get*"), 1));
outParams.Add(All.GetParameters(methods.FindByShortName("GetDlgItemText"), 1));
outParams.Add(All.GetParameters(methods.FindByShortName("getdlgtext"), 2));
outParams.Add(All.GetParameters(methods.FindByShortName("GetWindowText*"), 1));
result = All.GetByAncs(outParams);

CxList inMethods = methods.FindByMemberAccess("CCheckListBox.GetCheck");
result.Add(inMethods);

CxList dispFunctions = All.GetByAncs(All.GetParameters(methodDecl.FindByShortName("DISP_FUNCTION"), 2));
CxList dispPropertyEx = All.GetByAncs(All.GetParameters(methodDecl.FindByShortName("DISP_PROPERTY_EX"), 3));
CxList inputMethods = methodDecl.FindByShortName(dispFunctions + dispPropertyEx);
CxList dispatchMapInputs = All.GetParameters(inputMethods);
result.Add(dispatchMapInputs);