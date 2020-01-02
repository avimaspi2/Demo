CxList Inputs = Find_Interactive_Inputs();

CxList obj = Find_Object_Create().FindByShortName("File*");
CxList objPath = obj + All.FindAllReferences(obj);
objPath = All.GetByAncs(objPath) + objPath.GetAncOfType(typeof(AssignExpr));

CxList sanitize = Find_Path_Traversal_Sanitize() + Find_NonLeft_Binary(objPath);

result = obj.InfluencedByAndNotSanitized(Inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);