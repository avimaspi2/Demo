/*
	This query finds use of weak crypto algorithms in calls to the Kony crypto library
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyAll = Kony_All();
	CxList invokes = Find_Methods() * konyAll;
	
	CxList cryptoMethods = invokes.FindByName("kony.crypto.encrypt");
	cryptoMethods.Add(invokes.FindByName("kony.crypto.decrypt"));
	
	CxList algoParam = konyAll.GetParameters(cryptoMethods, 0);
	
	List<string> weakAlgo_Names = new List<string>{"tripledes"};
	CxList weakAlgo = Find_String_Literal().FindByShortNames(weakAlgo_Names);
	
	result = weakAlgo.DataInfluencingOn(algoParam);
}