if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList invokes = Find_Methods() * Kony_All();
	result.Add(invokes.FindByName("kony.db.executeSql"));
}