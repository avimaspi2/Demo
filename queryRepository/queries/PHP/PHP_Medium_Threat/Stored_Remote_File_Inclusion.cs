CxList db = Find_DB_Out() + Find_Read();

CxList methods = Find_Methods();
CxList include = methods.FindByShortNames(new List<string>{ "include", "include_once", "require", "require_once" }, false);

CxList numberSanitizer = Find_Integers();

result = include.InfluencedByAndNotSanitized(db, numberSanitizer);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);