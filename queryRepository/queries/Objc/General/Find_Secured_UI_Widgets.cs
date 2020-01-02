if (param.Length == 1)	// param[0] should be all references of UI widgets to check if they are secured
{
	CxList widgets = param[0] as CxList;
	if (widgets != null)
	{
		CxList secureMemberAccess = All.FindByMemberAccess("*.secureTextEntry"); 	// UIView.secureTextEntry = YES
		secureMemberAccess.Add(All.FindByMemberAccess("*.isSecureTextEntry"));		// UIView.isSecureTextEntry = true
		CxList setTrue = secureMemberAccess.GetAssigner().FindByShortName("true"); 	// 'YES' is parsed as 'true' and so does not need adittional treatment
		result = widgets * setTrue.GetAssignee().GetTargetOfMembers();
		CxList secureMethod = Find_Methods().FindByMemberAccess("*.setSecureTextEntry:"); // UIView.setSecureTextEntry(YES)
		CxList paramTrue = All.GetParameters(secureMethod).FindByShortName("true");
		result.Add(secureMethod.FindByParameters(paramTrue).GetTargetOfMembers());	// [UIView setSecureTextEntry:YES]
	}
}