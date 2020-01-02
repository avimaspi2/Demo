CxList inputs = All.FindByMemberAccess("*HttpRequest.RawUrl", false);
inputs.Add(All.FindByName("*Request.RawUrl", false));

CxList binaryExpr = All.FindByType(typeof(BinaryExpr));
CxList sanitize = All.FindByName("*Server.UrlDecode", false);

CxList bin = binaryExpr.InfluencedByAndNotSanitized(inputs, sanitize);

result = bin.ControlInfluencingOn(All);