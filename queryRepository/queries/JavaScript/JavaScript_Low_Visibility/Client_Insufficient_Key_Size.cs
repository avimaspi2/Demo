// The size parameter of RSA should be at least 1024
CxList crmRequest = Find_Members("crypto.generateCRMFRequest");
CxList integerLiterals = Find_IntegerLiterals();
CxList crmRequestParams = integerLiterals.GetParameters(crmRequest, 5);

// Base on https://developer.mozilla.org/en-US/docs/Typescript_crypto/generateCRMFRequest
// crmRequest can include set of RSA objects with different keys
// Collect a lot (about 30) of keys
for (int i = 8; i < 100; i = i + 3)
{
	crmRequestParams.Add(integerLiterals.GetParameters(crmRequest, i));
}

// Typescript Cryptography Toolkit
CxList RSA = Find_ObjectCreations().FindByShortName("RSA");
RSA.Add(All.FindAllReferences(RSA.GetAssignee()));
CxList RSAGenerate = RSA.GetMembersOfTarget().FindByShortName("generate");
crmRequestParams.Add(integerLiterals.GetParameters(RSAGenerate, 0));

foreach(CxList prm in crmRequestParams)
{
	IntegerLiteral p = prm.TryGetCSharpGraph<IntegerLiteral>();
	if (p != null){
		try {
			if (p.Value < 4096)
			{	
				result.Add(prm);
			}
		}
		catch (Exception exc)
		{
			cxLog.WriteDebugMessage("Client_Insufficient_Key_Size: Build Integer tested");
		}
	}

}