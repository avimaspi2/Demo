/*This Query looks for db Setters
The setters will be relevant to XSRF query and also to Parameter tampering Query*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList preparedStmt = XSAll.FindByShortName("prepareStatement");
	CxList definition = XSAll.FindByAssignmentSide(CxList.AssignmentSide.Left).GetByAncs(preparedStmt.GetFathers());
	CxList setter = XSAll.FindByShortName("set*");
	result.Add(setter.GetTargetOfMembers().FindAllReferences(definition).GetMembersOfTarget());
}