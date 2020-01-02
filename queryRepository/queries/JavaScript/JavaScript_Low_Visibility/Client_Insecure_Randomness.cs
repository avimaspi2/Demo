// Using by Math.random() is not secure if related to sensitive data such as some sort of encryption, cryptography,
// tokens, IDs, credentials

//Find Math.random
CxList random = Find_Members("Math.random").FindByType(typeof(MethodInvokeExpr));
// Find Mozilla RandomSource
CxList methods = Find_Methods();
random.Add(methods.FindByMemberAccess("crypto.getRandomValues")); // window.crypto.getRandomValues
random.Add(methods.FindByMemberAccess("msCrypto.getRandomValues")); // IE11
random.Add(methods.FindByMemberAccess("RandomSource.getRandomValues"));

//Vulnerable Data
CxList personal = Find_Personal_Info();
CxList crypto = Find_Encrypt();
CxList cookies = Find_Cookie();
CxList encodes = Find_Encode_Base64();
CxList dbs = Find_DB_In();
CxList logs = methods.FindByName("console.*");
logs = logs.FindByShortNames(new List<string>{"debug","error","info","log","trace","warn*"});
CxList sessions = Find_String_Short_Name(All, new List<string>{
		"XSRF_Token","XSRF-Token","sessionId","session_Id","session-Id", "sid"}, false); // sid ==> session id

//List with all data that cannot be influenced by Math.random
CxList vulnerableData = personal.Clone();
vulnerableData.Add(crypto);
vulnerableData.Add(cookies);
vulnerableData.Add(sessions);
vulnerableData.Add(encodes);
vulnerableData.Add(dbs);
vulnerableData.Add(logs);

result = vulnerableData.DataInfluencedBy(random);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);