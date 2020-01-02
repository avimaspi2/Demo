//find all executes that are called using ADONewConnection

CxList ado = Find_Methods().FindByShortName("ADONewConnection");

CxList unknownRef = All.FindByType(typeof(UnknownReference));
CxList left = unknownRef.GetByAncs(ado.GetAncOfType(typeof(AssignExpr)));
left = left.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList connection = unknownRef.FindAllReferences(left);
CxList executes = connection.GetMembersOfTarget().FindByShortName("Execute", false);

//find all access to result elements
left = unknownRef.GetByAncs(executes.GetAncOfType(typeof(AssignExpr)));
left = left.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList recordSet = unknownRef.FindAllReferences(left);

CxList access = recordSet.GetMembersOfTarget();
result.Add(access.FindByShortName("fields") + access.FindByShortNames(new List<String>(){ "get*", "fetch*", "cacheGet" }, false));