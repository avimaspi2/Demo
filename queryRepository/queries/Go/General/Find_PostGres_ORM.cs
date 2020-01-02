CxList methods = Find_Methods();
CxList unknownReferences = Find_UnknownReferences();

CxList connectionSQL = methods.FindByName("\"github.com/go-pg/pg\".Connect");
connectionSQL.Add(methods.FindByName("*pg.Connect"));

CxList connReferences = unknownReferences.FindAllReferences(connectionSQL.GetAssignee());
connReferences.Add(unknownReferences.FindByType("pg.DB"));

CxList rightMostMembers = connReferences.GetRightmostMember();
CxList preparedStmts = rightMostMembers.FindByShortName("Prepare").GetFathers().GetAssignee();
preparedStmts.Add(unknownReferences.FindAllReferences(preparedStmts));

CxList stmtReferences = unknownReferences.FindByType("pg.Stmt");

result.Add(stmtReferences);
result.Add(stmtReferences.GetMembersOfTarget());
result.Add(stmtReferences.GetRightmostMember());
result.Add(connReferences);
result.Add(connReferences.GetMembersOfTarget());
result.Add(rightMostMembers);
result.Add(preparedStmts);
result.Add(preparedStmts.GetMembersOfTarget());
result.Add(preparedStmts.GetRightmostMember());