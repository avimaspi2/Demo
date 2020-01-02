// Find all personal data that is sent to the output without encoding

CxList personal = Find_Personal_Info();
CxList outputs = Find_Write() + Find_DB_In();
CxList sanitize = Find_Methods().FindByShortName("encrypt");
result = personal.InfluencingOnAndNotSanitized(outputs, sanitize);

result -= result.DataInfluencedBy(result);