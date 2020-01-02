CxList personalInfo = Find_Personal_Info();
CxList logOutputs = Hapi_Find_Log_Write();

result = personalInfo.InfluencingOn(logOutputs).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);