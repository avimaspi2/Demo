CxList methods = Find_Methods();
CxList unknownRefs = Find_Unknown_References();
var encodingMethods = new List<string>(){
		"*to_base64*","*tobase64*",
		"*to_base32*","*tobase32*",
		"*to_hex*","*toHex*","*2hex*","hexStr",
		"*encode*"};
CxList encoding = methods.FindByShortNames(encodingMethods, false);

CxList hashing = Find_Hash_Functions_Outputs();
CxList encrypting = Find_Encrypt();

CxList cinRefs = unknownRefs.FindByShortNames(new List<string> {"cin", "wcin"});
CxList sensitiveData = Find_Personal_Info();
CxList passwords = Find_All_Passwords();
sensitiveData -= passwords;

CxList sdDecls = sensitiveData.FindByType(typeof(Declarator));
CxList passDecls = passwords.FindByType(typeof(Declarator));

sensitiveData = cinRefs.GetAssignee() * sensitiveData;
sensitiveData.Add(sdDecls);

passwords = cinRefs.GetAssignee() * passwords;
passwords.Add(passDecls);

CxList encodingParams = Find_MethodDecls().FindByShortNames(encodingMethods);
encodingParams = All.GetParameters(encodingParams).FindByTypes(new string[]{"char", "*string"});

CxList hashingParams = All.GetByAncs(All.GetParameters(hashing.GetAncOfType(typeof(MethodInvokeExpr))));
CxList unsafe_passwords = passwords.InfluencingOn(encoding + encodingParams)
	- passwords.InfluencingOn(hashingParams);

CxList unsafe_data = sensitiveData.InfluencingOn(encoding + encodingParams)
	- sensitiveData.InfluencingOn(encrypting);

result = unsafe_passwords;
result.Add(unsafe_data);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);