CxList Inputs = Find_Interactive_Inputs();

CxList obj = Find_Object_Create().FindByShortName("File*");
CxList sanitize = Find_Path_Traversal_Sanitize();

CxList fullResult = obj.InfluencedByAndNotSanitized(Inputs, sanitize);

CxList objPath = obj + All.FindAllReferences(obj);
objPath = All.GetByAncs(objPath) + objPath.GetAncOfType(typeof(AssignExpr));
sanitize.Add(Find_NonLeft_Binary(objPath));
result = fullResult - fullResult.InfluencedByAndNotSanitized(Inputs, sanitize);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);