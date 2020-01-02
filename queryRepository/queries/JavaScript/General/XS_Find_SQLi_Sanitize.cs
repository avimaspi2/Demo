/*This query will look for sanitation for SQL injetion- Only first parameter  of DB_In methods
can be vulnerable to SQLi so the rest canbe added to sanitizers */

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList dbIn = XS_Find_DB_In();
	CxList XSAll = XS_Find_All();
	CxList method = XSAll.FindByParameters(dbIn);
	CxList firstParamsOnly = dbIn.GetParameters(method, 0);
	result = dbIn - firstParamsOnly;
}