CxList inputs = All.FindByMemberAccess("*HttpServletRequest.getRequestURI") +
	All.FindByMemberAccess("*HttpServletRequest.getRequestURL") +
	All.FindByMemberAccess("*HttpServletRequest.getServletPath");

CxList sanitize = 
	All.FindByMemberAccess("URLDecoder.decode") +
	All.FindByMemberAccess("Encoder.decodeForURL") + 
	Get_ESAPI().FindByMemberAccess("Encoder.canonicalize");

CxList binaryExpr = All.FindByType(typeof(BinaryExpr));

CxList bin = binaryExpr.InfluencedByAndNotSanitized(inputs, sanitize);

result = bin.ControlInfluencingOn(All);