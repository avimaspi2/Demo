/*
	This query finds flows from hardcoded strings to their use as an encryption key
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyAll = Kony_All();
	CxList invokes = Find_Methods() * konyAll;
	
	CxList cryptoCalls = invokes.FindByName("kony.crypto.encrypt");
	cryptoCalls.Add(invokes.FindByName("kony.crypto.decrypt"));
	
	CxList cryptoKey_Params = konyAll.GetParameters(cryptoCalls, 1);
	
	CxList sources = Find_String_Literal() * konyAll;
	CxList sinks = cryptoKey_Params.Clone();
	CxList sanitizers = invokes.FindByName("kony.crypto.newKey");
	
	result = sources.InfluencingOnAndNotSanitized(sinks, sanitizers);
}