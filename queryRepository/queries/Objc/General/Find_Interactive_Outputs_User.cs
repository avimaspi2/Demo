CxList UIwidgets = Find_UI_Widgets();

CxList textOutputs = UIwidgets.FindByType("UITextField");	
textOutputs.Add(UIwidgets.FindByType("UITextView"));
textOutputs.Add(UIwidgets.FindByType("UILabel"));

textOutputs = textOutputs.GetMembersOfTarget();

//Pasteboard
CxList pasteBoard = All.FindByType("UIPasteboard");
pasteBoard = pasteBoard.GetMembersOfTarget();
pasteBoard.Add(pasteBoard.GetMembersOfTarget());
pasteBoard.Add(pasteBoard.GetMembersOfTarget());

List<string> allMethods = new List<string> {
		"*string*","*items*","*URL*",
		"*setData:forPasteboardType:*","*setValue:forPasteboardType:*"
		};

pasteBoard = pasteBoard.FindByShortNames(allMethods);

//Add assign statements to textOutputs and pasteBoard outputs.
CxList relAssigns = textOutputs.FindByShortName("text*");
relAssigns.Add(pasteBoard);

CxList assigns = All.NewCxList();
assigns.Add(relAssigns);
assigns = assigns.GetFathers().FindByType(typeof(AssignExpr));

CxList leftSide = Find_UnknownReference();
leftSide.Add(Find_MemberAccess());

leftSide = leftSide.FindByAssignmentSide(CxList.AssignmentSide.Left);
assigns = leftSide.GetByAncs(assigns);
assigns *= relAssigns;

result.Add(textOutputs.FindByShortName("setText*"));
result.Add(All.GetParameters(pasteBoard, 0));
result.Add(assigns);