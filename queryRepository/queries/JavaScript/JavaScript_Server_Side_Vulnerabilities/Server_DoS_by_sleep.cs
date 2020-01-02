// Find all Sleeps, who influenced by User Input, so User can send number to Sleep, causing DoS
CxList inputs = NodeJS_Find_Interactive_Inputs();

CxList methods = Find_Methods();
CxList sleepMeth = methods.FindByShortNames(new List<string>{"setTimeout","setInterval"});
CxList sleep = All.GetParameters(sleepMeth, 1);
sleep.Add(sleep.GetTargetOfMembers());
sleep.Add(sleep.GetTargetOfMembers());
sleep.Add(sleep.GetTargetOfMembers());
CxList sanitizer = NodeJS_Find_DB_Base();
// return this, who influenced by Input and not sanitized
result = sleep.InfluencedByAndNotSanitized(inputs, sanitizer);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);;