// Query Stored_File_Manipulation
// ==============================
// In deference from File manipulation, in stored file manipulation the input from the file (Find_Readapplied)
CxList Inputs = Find_Read();

CxList obj = Find_Object_Create().FindByShortName("File*");

CxList objPath = obj + All.FindAllReferences(obj);
objPath = All.GetByAncs(objPath) + objPath.GetAncOfType(typeof(AssignExpr));
CxList sanitize = Find_Path_Traversal_Sanitize();
result = obj.InfluencedByAndNotSanitized(Inputs, sanitize);

sanitize.Add(Find_NonLeft_Binary(objPath));
result -= result.InfluencedByAndNotSanitized(Inputs, sanitize);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);