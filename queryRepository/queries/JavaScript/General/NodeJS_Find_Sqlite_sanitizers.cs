//Gets the sanitizers that uses an intermediate statement
CxList dbPrepare = NodeJS_Find_Sqlite_commands().FindByShortName("prepare");
CxList stmtAssign = Find_Assign_Lefts().GetByAncs(dbPrepare.GetAncOfType(typeof(AssignExpr)));
CxList statements = All.FindAllReferences(stmtAssign);
result = statements.GetMembersOfTarget().FindByShortNames(new List<string>{"bind","run","get","all","each"});