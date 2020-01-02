CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccess("Response.addHeader"));
result.Add(methods.FindByMemberAccess("Response.setHeader"));

// ESAPI
result.Add(methods.FindByMemberAccess("HTTPUtilities.safeSetHeader"));

// Akka
result.Add(methods.FindByName("*.addHeader"));
result.Add(methods.FindByName("*.addHeaders"));