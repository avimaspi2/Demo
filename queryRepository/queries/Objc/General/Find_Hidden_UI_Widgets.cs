CxList sensitiveWidgets = Find_UI_Widgets_With_Sensitive_Data();

CxList widgetMembers = sensitiveWidgets.GetMembersOfTarget();

CxList widgetHidden = widgetMembers.FindByShortName("hidden");
widgetHidden.Add(widgetMembers.FindByShortName("setHidden:"));

CxList trueFalse = All.FindByFathers(widgetHidden.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Right);

CxList yes = trueFalse.FindByShortName("true", false);
yes.Add(trueFalse.FindByShortName("0"));

CxList hiddenYes = widgetHidden.FindByFathers(yes.GetAncOfType(typeof(AssignExpr))).GetTargetOfMembers();

CxList yesParameter = All.GetParameters(widgetHidden).FindByShortName("true", false);
yesParameter.Add(All.GetParameters(widgetHidden).FindByShortName("0"));

hiddenYes.Add(widgetHidden.FindByParameters(yesParameter).GetTargetOfMembers());

CxList widgetAlpha = widgetMembers.FindByShortName("alpha");
widgetAlpha.Add(widgetMembers.FindByShortName("setAlpha:"));

trueFalse = All.FindByFathers(widgetAlpha.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList zero = trueFalse - trueFalse.FindByShortName("0");

CxList hiddenZero = widgetAlpha.FindByFathers(zero.GetAncOfType(typeof(AssignExpr))).GetTargetOfMembers();

CxList zeroParameter = All.GetParameters(widgetAlpha);
zeroParameter -= zeroParameter.FindByShortName("0");

hiddenZero.Add(widgetAlpha.FindByParameters(zeroParameter).GetTargetOfMembers());

result = hiddenYes + 
	(widgetAlpha.GetTargetOfMembers() - hiddenZero);