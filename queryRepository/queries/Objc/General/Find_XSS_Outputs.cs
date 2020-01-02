CxList methods = Find_Methods();

CxList output=methods.FindByShortName("*setHTTPBody:*").GetFathers();

//URL Scheme
CxList schemes = methods.FindByShortName("*openURL:*");

result.Add(output);
result.Add(schemes);