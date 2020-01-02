CxList regexInFirstParam = Find_Match();
regexInFirstParam.Add(Find_Replace_Regex_In_First_Param());
regexInFirstParam.Add(All.FindByName("Pattern.compile"));

CxList inputs = Find_Inputs();

//Explicitly remove System.getProperty("line.separator") from the input list
CxList systemGetProperty = inputs.FindByMemberAccess("System.getProperty");
CxList lineSeparator = All.FindByShortName("line.separator");
CxList sanitized = systemGetProperty.FindByParameters(lineSeparator); 
inputs -= sanitized; 

result = Find_ReDoS(inputs, regexInFirstParam, 0, false);

CxList regexInSecondParam = Find_Replace_Regex_In_Second_Param();
result.Add(Find_ReDoS(inputs, regexInSecondParam,1, false));