CxList inputs = Find_Inputs();
CxList methods = Find_Methods();
CxList system =  methods.FindByShortName("SYSTEM");
CxList calls = Find_CALL_Statements();

CxList commandExecution = system.GetByAncs(calls);

result = inputs.InfluencingOn(commandExecution);