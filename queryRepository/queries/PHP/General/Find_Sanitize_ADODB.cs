//find all executes that are called using ADONewConnection

CxList ado = All.FindByShortName("ADONewConnection").FindByType(typeof(MethodInvokeExpr));

CxList unknownRef = All.FindByType(typeof(UnknownReference));
CxList left = unknownRef.GetByAncs(ado.GetAncOfType(typeof(AssignExpr)));
left = left.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList connection = unknownRef.FindAllReferences(left);
CxList prepare = All.GetByAncs((All.GetParameters(connection.GetMembersOfTarget().FindByShortName("Execute", false), 1)));
result.Add(prepare);