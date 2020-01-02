/*This query will look for any update operation on the XSDS*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList update = XS_Find_XSDS();
	List<string> names = new List<string>(new string[]{"$delete","$discardAll","$saveAll","$upsert"});
		
	CxList methods = Find_Methods() * XSAll;
	CxList dbInvokes = methods.FindByShortNames(names);
	CxList PotentialUpdates = dbInvokes.DataInfluencedBy(update);

	result.Add(XSAll.GetByAncs(XSAll.GetParameters(PotentialUpdates, 0)));
}