// Query Client_Side_Injection
// //////////////////////////-
// This query finds possible SQL injections flows in the client side 

// Find DB access
CxList db = Find_DB_In();

// Find URL inputs (for protocol handlers)
CxList mapUriMethod = All.FindByShortName("MapUri");
CxList inputUrls = All.GetParameters(mapUriMethod, 0);

// Find external inputs (SMS, protocol handlers)
CxList inputs = All.FindByMemberAccess("ISmsTextMessage.Body") + inputUrls;

CxList sanitize = Find_Sanitize();

// Find SQL injection
result = db.InfluencedByAndNotSanitized(inputs, sanitize);

// Find XSS is not implemented at this stage.
//CxList xssSanitize = Find_XSS_Sanitize();
//CxList browser = All.FindByType("WebBrowser");