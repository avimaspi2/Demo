// HttpsURLConnection references
CxList members = Find_Members();
CxList httpsTargets = Find_UnknownReference();
httpsTargets.Add(Find_Declarators());
httpsTargets.Add(Find_TypeRef());
result.Add(httpsTargets.FindByType("HttpsURLConnection"));

// OKHttp
CxList tlsSanitization = members.FindByMemberAccess("ConnectionSpec.MODERN_TLS");
tlsSanitization.Add(members.FindByMemberAccess("ConnectionSpec.COMPATIBLE_TLS"));
result.Add(tlsSanitization);