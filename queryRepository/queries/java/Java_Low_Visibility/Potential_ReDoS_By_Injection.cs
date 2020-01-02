CxList inputs = Find_Inputs();
CxList regexInFirstParam = Find_Match();

regexInFirstParam.Add(Find_Replace_Regex_In_First_Param());
result = Find_ReDoS(inputs, regexInFirstParam, 0, true);

CxList regexInSecondParam = Find_Replace_Regex_In_Second_Param();
result.Add(Find_ReDoS(inputs, regexInSecondParam, 1, true));