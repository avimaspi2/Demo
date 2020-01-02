CxList personalInfo = Find_Personal_Info();
CxList cookieSet = Hapi_Find_Cookie_Set();
CxList sanitizers = NodeJS_Find_Encrypt();

result = personalInfo.InfluencingOnAndNotSanitized(cookieSet, sanitizers).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
result.Add(personalInfo * cookieSet - sanitizers);