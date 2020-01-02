CxList XPath = Find_XPath_Output();
CxList inputs = Find_Read() + Find_DB_Out();
CxList sanitized = Find_Sanitize();

result = XPath.InfluencedByAndNotSanitized(inputs, sanitized);