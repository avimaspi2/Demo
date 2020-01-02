CxList redirectFunctions = Find_Methods().FindByShortName("header", false);
CxList redirectLocationStrings = Find_Strings().FindByShortName("*location*", false);

redirectFunctions = redirectFunctions.InfluencedBy(redirectLocationStrings);
CxList inputs = Find_Interactive_Inputs();

CxList sanitize = Find_General_Sanitize();

result = redirectFunctions.InfluencedByAndNotSanitized(inputs, sanitize);