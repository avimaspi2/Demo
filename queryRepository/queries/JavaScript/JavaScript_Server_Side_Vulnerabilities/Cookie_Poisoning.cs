CxList inputs = NodeJS_Find_Interactive_Inputs();
CxList cookiesSet = Hapi_Find_Cookie_Set();
CxList sanitize = NodeJS_Find_Encrypt();

result = inputs.InfluencingOnAndNotSanitized(cookiesSet, sanitize);
result.Add(inputs * cookiesSet - sanitize);