CxList Databases = Find_BetterSqlite3Lib_Databases();

CxList Statements = Databases.GetMembersOfTarget().FindByShortName("prepare");
Statements.Add(All.FindAllReferences(Statements.FindByAssignmentSide(CxList.AssignmentSide.Right).GetAssignee()));

result = Statements;