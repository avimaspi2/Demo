/*
This query will look for DB_Out
DB_Out will be considered one of the following:
a. the invocation of a stored procedure
b. The invocation of executeQuery 
c. The invocation of getResultSet
*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList methods = Find_Methods() * XSAll;
	CxList oldApiProcedure = (methods.FindByShortName("procedure") * XS_Find_XSDS()).GetTargetOfMembers();
	CxList loadProc = methods.FindByShortName("loadProcedure");
	loadProc.Add(oldApiProcedure);
		
	CxList fathers = loadProc.GetFathers();
	CxList storedProcedure = XSAll.FindByAssignmentSide(CxList.AssignmentSide.Left).GetByAncs(fathers);
	CxList invocationOfSP = XSAll.FindByType(typeof(MethodInvokeExpr)).FindAllReferences(storedProcedure);
	result.Add(invocationOfSP);
	//cover  regular results of hdb and db
	CxList execute = methods.FindByShortNames(new List<string>{"executeQuery","getResultSet"});
		
	result.Add(execute);
}