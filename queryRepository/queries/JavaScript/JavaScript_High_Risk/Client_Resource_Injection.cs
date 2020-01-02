// Query Resource_Injection
// ========================

CxList socket = Find_ObjectCreations().FindByShortName("WebSocket");
CxList inputs = Find_Inputs();
CxList sanitize = Sanitize();

result = inputs.InfluencingOnAndNotSanitized(socket, sanitize);