CxList db = Find_Dynamic_DB_In();
CxList methods = Find_Methods();
CxList usingMethods = methods.FindByName("using");
CxList inputs = Find_Inputs();
CxList sanitized = Find_Sanitize();

CxList executeImmediate = db.FindByName("execute_immediate");
//take from the list, the methods that use using
CxList executeImmediateSanitize = executeImmediate.DataInfluencingOn(usingMethods).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
executeImmediate -= executeImmediateSanitize;
CxList paramExecuteImmediate = All.GetParameters(executeImmediate).DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);


CxList possibleSqlQuery = Find_BinaryExpr().DataInfluencedBy(Find_String_Literal());
CxList possibleBadParameter = Find_UnknownReference().DataInfluencingOn(possibleSqlQuery).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
inputs = inputs.FindAllReferences(possibleBadParameter);
inputs.Add(paramExecuteImmediate);

result = db.InfluencedByAndNotSanitized(inputs, sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);