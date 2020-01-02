CxList UIwidgets = Find_UI_Widgets();

CxList textInputs = UIwidgets.FindByTypes(new string[]{"UITextField","UITextView"});
textInputs = textInputs.GetMembersOfTarget();

CxList alertView = All.FindByMemberAccess("UIAlertView.textFieldAtIndex:");

//Pasteboard
CxList pasteBoard = All.FindByType("UIPasteboard");
pasteBoard = pasteBoard.GetMembersOfTarget();
pasteBoard.Add(pasteBoard.GetMembersOfTarget());
pasteBoard.Add(pasteBoard.GetMembersOfTarget());

CxList pasteBoardNames = pasteBoard.FindByShortName("*valueForPasteboardType:*");
pasteBoardNames.Add(pasteBoard.FindByShortName("*dataForPasteboardType:*"));
pasteBoardNames.Add(pasteBoard.FindByShortName("*itemSetWithPasteboardTypes:*"));

CxList assigns = textInputs.FindByShortName("text*");
assigns = assigns.GetFathers().FindByType(typeof(AssignExpr));

CxList leftSide = Find_UnknownReference();
leftSide.Add(Find_MemberAccesses());

leftSide = leftSide.FindByAssignmentSide(CxList.AssignmentSide.Left);
assigns = leftSide.GetByAncs(assigns);

result.Add(textInputs.FindByShortName("text"));

List<string> methodsNames = new List<string>() {"*string*", "*items*", "*URL*"};
result.Add(pasteBoardNames.FindByShortNames(methodsNames));

result.Add(All.GetParameters(pasteBoardNames, 0));
result.Add(alertView);

result -= assigns;
result -= result.FindByAssignmentSide(CxList.AssignmentSide.Left);