/*looks for outputs as Secure_Store_Read*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList objectCreate = XSAll.FindByType(typeof(ObjectCreateExpr));
	CxList secureStore = objectCreate.FindByName("*security.Store");
	CxList storeDeclarator = XSAll.FindByAssignmentSide(CxList.AssignmentSide.Left).GetByAncs(secureStore.GetFathers());
	CxList methods = Find_Methods() * XSAll;
	CxList read = methods.FindByShortNames(new List<string>{"read","readForUser"});		
	result = read.GetTargetOfMembers().FindAllReferences(storeDeclarator).GetMembersOfTarget();
}