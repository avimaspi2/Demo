CxList inputs = All.FindByMemberAccess("*HttpServletRequest.getRequestURI");
inputs.Add(All.FindByMemberAccess("*HttpServletRequest.getRequestURL"));
inputs.Add(All.FindByMemberAccess("*HttpServletRequest.getServletPath"));

CxList sanitize = All.FindByMemberAccess("URLDecoder.decode");
sanitize.Add(All.FindByMemberAccess("Encoder.decodeForURL"));
sanitize.Add(Get_ESAPI().FindByMemberAccess("Encoder.canonicalize"));

CxList binaryExpr = Find_BinaryExpr();

CxList bin = binaryExpr.InfluencedByAndNotSanitized(inputs, sanitize);

result = bin.ControlInfluencingOn(All);