CxList cond = All.GetByAncs(Find_Conditions());
CxList methods = Find_Methods();
CxList ip = methods.FindByShortNames(new List<String>()
	{ "checkdnsrr", "dns_check_record", "dns_get_mx", "dns_get_record", "gethostbyaddr", "getmxrr" });

result = ip.DataInfluencingOn(cond) + cond * ip;
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);