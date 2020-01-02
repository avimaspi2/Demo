CxList pi = Find_Personal_Info();

pi -= pi.FindByType(typeof(Declarator));
CxList emptyString = Find_String_Literal().FindByShortName("");
CxList assign = emptyString.GetFathers().FindByType(typeof(AssignExpr));
pi -= pi.FindByFathers(assign);
pi -= pi.FindByShortName("*Error");

CxList inputs = NodeJS_Find_Inputs() + NodeJS_Find_DB_Out();
pi = pi.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly) + 
	pi * inputs;

CxList outputs = NodeJS_Find_Interactive_Outputs();
CxList sanitizers = Find_Integers();
sanitizers.Add(Find_Encrypt());
result = outputs.InfluencedByAndNotSanitized(pi, sanitizers).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
CxList infoOut = outputs * pi;  
result.Add(infoOut);