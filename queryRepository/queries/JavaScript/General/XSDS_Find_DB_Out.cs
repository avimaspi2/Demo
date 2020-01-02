/*This query will look for the result of any query for XSDS*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList xsdbIn = XSDS_Find_DB_In();

	xsdbIn = XSAll.FindByParameters(xsdbIn);

	//those will be covered by $execute
	List<string> queryNames = new List<string>(new string[]{"$addFields","$aggregate","$project","$where","$matching"});

	CxList dbOut = xsdbIn - xsdbIn.FindByShortNames(queryNames);

	result.Add(dbOut);
	//handle the $execute of XSDS query 
	CxList methods = Find_Methods() * XSAll;
	result.Add(methods.FindByShortName("$execute"));
}