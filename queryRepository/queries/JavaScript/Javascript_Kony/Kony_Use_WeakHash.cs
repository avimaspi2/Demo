/*
	This query finds use of weak hash algorithms in calls to the Kony crypto library
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyAll = Kony_All();
	CxList invokes = Find_Methods() * konyAll;
	
	CxList cryptoHashMethods = invokes.FindByName("kony.crypto.createHash");
	cryptoHashMethods.Add(invokes.FindByName("kony.crypto.createHMacHash"));
	
	CxList algoParam = konyAll.GetParameters(cryptoHashMethods, 0);
	
	List<string> weakAlgo_Names = new List<string>{"sha1", "md5"};
	CxList weakAlgo = Find_String_Literal().FindByShortNames(weakAlgo_Names);
	
	result = weakAlgo.DataInfluencingOn(algoParam);
}