/// <summary>
///  This query returns any UITextField and UITextView that holds sensitive information
/// 	in UIApplications that allow the use of third party keyboards
/// </summary>


CxList apps = Find_AppDelegate_Class();
CxList unprotectedApps = Find_Extn_Keyboard_Enabled();

if (apps.Count == unprotectedApps.Count) // if there are no protected apps then check for other protections.
{
	CxList UIUnprotected = Find_UI_Widgets_With_Sensitive_Data(); // find UI Views that are not in a protected App
	CxList personalInfo = UIUnprotected.FindByTypes(new string[]{"UITextField", "UITextView"});
	personalInfo = personalInfo - UIUnprotected.FindAllReferences(Find_Secured_UI_Widgets(personalInfo));
	
	CxList UIProtected = Find_Secured_UI_Widgets(All.FindByTypes(new string[]{"UITextField", "UITextView"}));
	CxList allRefsProtected = All.FindAllReferences(UIProtected);
	
	//Get assignment expressions from personalInfo that are on the left side
	CxList ancLeft = personalInfo.FindByAssignmentSide(CxList.AssignmentSide.Left).GetAncOfType(typeof(AssignExpr));
	//Get assignment expressions from allRefsProtected
	CxList ancsProtected = allRefsProtected.GetAncOfType(typeof(AssignExpr));
	CxList allAncs = ancLeft * ancsProtected;
	//Get the ones that are sanitized by assignment
	CxList assignProtected = All.FindByFathers(allAncs) * personalInfo;
	CxList allRefsAssignProtected = personalInfo.FindAllReferences(assignProtected);
	
	CxList removeList = All.NewCxList();
	foreach (CxList item in personalInfo.GetCxListByPath())
	{
		if (item.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes).Count == 1) 
		{
			if (item.FindByType(typeof(ParamDecl)).Count > 0)
			{
				removeList.Add(item); // remove single nodes of type ParamDecl from results
			}
		}
	}
	removeList.Add(allRefsAssignProtected);
	result = personalInfo - removeList; 
}