if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyAll = Kony_All();
	CxList invokes = Find_Methods() * konyAll;
	CxList executeSql = invokes.FindByName("kony.db.executeSql");
	CxList successCallback = konyAll.GetParameters(executeSql, 3);
	CxList successCallbackDefinition = konyAll.FindDefinition(successCallback);

	CxList resultset = konyAll.GetParameters(successCallbackDefinition, 1);

	result = resultset;
}