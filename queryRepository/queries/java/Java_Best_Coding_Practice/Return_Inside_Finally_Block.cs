CxList AllReturn = All.GetAncOfType(typeof(ReturnStmt));
CxList AllTry = All.GetAncOfType(typeof(TryCatchFinallyStmt));
CxList AllFinally = All.GetFinallyClause(AllTry);
CxList AllReturnInFinally = AllReturn.GetByAncs(AllFinally);

result = AllReturnInFinally;