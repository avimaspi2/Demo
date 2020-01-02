/// Find Empty TryBlocks
CxList tryCatch = Find_TryCatchFinallyStmt();
foreach (CxList item in tryCatch) {
	TryCatchFinallyStmt tcfs = item.TryGetCSharpGraph<TryCatchFinallyStmt>();
	StatementCollection sc = tcfs.Try;
	if (sc.Count == 0) {
		result.Add(item);
	}
}