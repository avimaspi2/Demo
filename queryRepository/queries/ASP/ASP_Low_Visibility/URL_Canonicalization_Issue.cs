CxList inputs = 	All.FindByMemberAccess("*httprequest.rawurl") +
					All.FindByName("*request.rawurl");

CxList binaryExpr = All.FindByType(typeof(BinaryExpr));
CxList sanitize = All.FindByName("*server.urldecode");

CxList bin = binaryExpr.InfluencedByAndNotSanitized(inputs, sanitize);

result = bin.ControlInfluencingOn(All);