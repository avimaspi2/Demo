CxList password = Find_Passwords();
CxList stringLiteral = Find_StringLiterals_For_Hardcoded_Password();
CxList methods = Find_Methods();
CxList objectCreatExpr = Find_ObjectCreations();

CxList stringAsParameter = stringLiteral.GetParameters(methods.FindByShortName("getElementById"));
stringLiteral -= stringAsParameter;


CxList suspectObjectCreateExpr = password.GetAncOfType(typeof(ObjectCreateExpr));
CxList nonSuspectObjectCreateExpr = stringLiteral.GetByAncs(objectCreatExpr - suspectObjectCreateExpr);
stringLiteral -= nonSuspectObjectCreateExpr;

List<string> stringDelimitersMethods = new List<string>()
	{ "split", "replace", "endsWith", "startsWith", "match", "indexOf" , "lastIndexOf", "localeCompare", "search" };

List<string> cryptoParamsToDiscard = new List<string>()
	{ "ascii", "utf8", "utf16le", "ucs2", "base64", "latin1", "binary", "hex" , "md5", "sha1", "sha256" };

List<string> cryptoFunctions = new List<string>()
	{ "digest", "createHash" };

CxList cryptoParamsLiterals = stringLiteral.FindByShortNames(cryptoParamsToDiscard);
CxList cryptoMethods = methods.FindByShortNames(cryptoFunctions, false);
CxList cryptoParams = cryptoParamsLiterals.GetParameters(cryptoMethods, 0);

CxList sanitizers = Find_Integers();
sanitizers.Add(stringLiteral.GetParameters(methods.FindByShortNames(stringDelimitersMethods, false)));
sanitizers.Add(cryptoParams);

// Since 'ParamDecl' has no flow, we search by fathers with the 'StringLiteral' 
CxList passParamDecl = password.FindByType(typeof(ParamDecl));
password -= passParamDecl;
result = stringLiteral.FindByFathers(passParamDecl);

result.Add(password.InfluencedByAndNotSanitized(stringLiteral, sanitizers)
	.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow));