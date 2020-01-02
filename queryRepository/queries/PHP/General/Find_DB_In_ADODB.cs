//find all executes that are called using ADONewConnection

CxList ado = Find_Methods().FindByShortName("ADONewConnection");

CxList unknownRef = All.FindByType(typeof(UnknownReference));
CxList left = unknownRef.GetByAncs(ado.GetAncOfType(typeof(AssignExpr)));
left = left.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList connection = unknownRef.FindAllReferences(left);
CxList action = connection.GetMembersOfTarget();
result.Add(action.FindByShortNames(new List<String>(){ "Execute", "SelectLimit" }, false));