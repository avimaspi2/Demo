CxList variables = All.FindByType(typeof(Dom.UnknownReference));
CxList cookies = variables.FindByShortNames(new List<String>(){ "_COOKIE", "HTTP_COOKIE_VARS" });
CxList cond = All.GetByAncs(Find_Conditions());

result = cond.DataInfluencedBy(cookies);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);