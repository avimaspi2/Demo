/*Find all relevant DB_IN for XS $.db and $.hdb*/
/*This query will mark ALL parameters of the below listed methods as the db_in*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList methods = Find_Methods() * XSAll;
	CxList prepareStmt = methods.FindByShortName("prepareStatement");
	CxList hdbExecuteQuery = methods.FindByShortName("executeQuery");
	CxList hdbExecuteUpdate = methods.FindByShortName("executeUpdate");
	CxList prm = XSAll.FindByType(typeof(Param));
		
	CxList allStmt = All.NewCxList();
	allStmt.Add(hdbExecuteQuery);
	allStmt.Add(prepareStmt);
	allStmt.Add(hdbExecuteUpdate);
		
	result.Add((XSAll - prm).GetParameters(allStmt));
}