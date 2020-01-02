CxList inputs = 	All.FindByMemberAccess("*HttpRequest.RawUrl") +
					All.FindByName("*Request.RawUrl");

CxList binaryExpr = All.FindByType(typeof(BinaryExpr));
CxList sanitize = All.FindByName("*Server.UrlDecode");

CxList bin = binaryExpr.InfluencedByAndNotSanitized(inputs, sanitize);

result = bin.ControlInfluencingOn(All);