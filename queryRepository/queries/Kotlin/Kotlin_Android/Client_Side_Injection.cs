CxList interactiveInputs = Find_Interactive_Inputs() ;
CxList dbIn = Find_DB_In();
CxList firstParameter = All.GetParameters(dbIn, 0);
result = firstParameter.InfluencedByAndNotSanitized(interactiveInputs, Find_Android_Sanitize());