//find all XSDS Access- import entity, using dbutils and using stored procedures

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList methods = Find_Methods() * XSAll;
	CxList relevantImport = methods.FindByShortName("import");
	//handle the dbutils procedure
	CxList oldApiProcedure = XSAll.FindByShortName("sap.hana.xs.libs.dbutils").GetParameters(relevantImport);
	relevantImport = relevantImport.FindByParameters(oldApiProcedure);
	CxList declaration = XSAll.FindByAssignmentSide(CxList.AssignmentSide.Left).GetByAncs(relevantImport.GetFathers());
	CxList actions = XSAll.FindAllReferences(declaration).GetMembersOfTarget();
	result = actions;
	result.Add(XSAll.FindByShortName("$importEntity"));
}