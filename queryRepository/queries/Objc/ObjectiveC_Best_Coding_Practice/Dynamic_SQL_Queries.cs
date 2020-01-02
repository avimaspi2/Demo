//////////////////////////////////////////////////////////////////////////////////
// Query: Dynamic_SQL_Queries
// Purpose: Find SQL queries that are built of concatenation of strings
//
 
List<string> appendMethods = new List<string> { 
		// C String
		"sprintf", "snprintf", "strcat", "strncat",
		// NSArray
		"componentsJoinedByString:*",
		// NSMutableString
		"appendString:*",
		"appendFormat:*",
		"insertString:atIndex:*",
		"replaceCharactersInRange:withString:*",
		"replaceOccurrencesOfString:withString:options:range:*",
		// NSString
		"initWithFormat:*",
		"stringWithFormat:*",
		"stringByAppendingString:*",
		"appending:*",
		"appendingFormat",
		"stringByAppendingFormat:*",	
		"localizedStringWithFormat:*",
		"stringByReplacingOccurrencesOfString:withString:*",
		"stringByReplacingCharactersInRange:withString:*",
		"stringByReplacingCharactersInRange:with:*",
		"replacingOccurrences:*",
		"replacingCharacters:*"
		};

 
CxList db = Find_SQL_DB_In();
CxList methods = Find_Methods();
CxList methodsConcatenate = methods.FindByShortNames(appendMethods);

methodsConcatenate.Add(methods.FindByShortName("NSString*").FindByParameterName("format", 0));
methodsConcatenate.Add(methods.FindByShortName("String*").FindByParameterName("CString", 0));
methodsConcatenate.Add(methods.FindByShortName("stringByAppendingFormat").FindByParameterName("format", 0));

CxList sanitize = Find_SQL_Sanitize();

result = methodsConcatenate.InfluencingOnAndNotSanitized(db, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow)
	.ReduceFlowByPragma();
result.Add(db * methodsConcatenate);