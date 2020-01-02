/*This query will look for modifying/querying XSDS- it will return the parameter*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	result = XSDS_Find_DB_Update();
	List<string> names = new List<string>(new string[]{"$field","$find*","$get","$select"});
	CxList methods = Find_Methods() * XSAll;
	CxList dbInvokes = methods.FindByShortNames(names);
	CxList PotentialQueries = dbInvokes.DataInfluencedBy(XS_Find_XSDS());

	//QueryObj
	//we will mark the following list parameters as DB-IN
	List<string> queryNames = new List<string>(new string[]{"$addFields","$project","$where","$matching"});
	PotentialQueries.Add(methods.FindByShortNames(queryNames));
	result.Add(All.GetByAncs(XSAll.GetParameters(PotentialQueries, 0)));
}