/*This will activate the Find_Integers query in Xs only context*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	result = Find_Integers() * XSAll;
	//db executeUpdate also returns an integer
	CxList methods = Find_Methods() * XSAll;
	CxList executeUpdate = methods.FindByShortName("executeUpdate");
	CxList parentOfExecuteUpdate = executeUpdate.GetFathers();
	//we are interested in either Declarator or left side of assignement as a sanitizer
	CxList xsDBIn = XS_Find_DB_In();
	result.Add(parentOfExecuteUpdate.FindByType(typeof(Declarator)).DataInfluencedBy(xsDBIn));
	CxList leftSideOfUpdate = (XSAll.FindByType(typeof(UnknownReference)).FindByAssignmentSide(CxList.AssignmentSide.Left).FindByFathers(parentOfExecuteUpdate));
	result.Add(leftSideOfUpdate.DataInfluencedBy(xsDBIn).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));
}