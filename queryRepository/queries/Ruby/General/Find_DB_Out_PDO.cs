CxList methods = Find_Methods();

// 1 - Explicite DB function names

// 2.1 - Implicit DB function names influenced by PDO
CxList PDO = All.FindByShortName("PDO");
CxList PrepareMethods = methods.FindByShortName("prepare");
CxList QueryMethods = methods.FindByShortName("query");
CxList PreparePDO = PrepareMethods.DataInfluencedBy(PDO);
CxList QueryPDO = QueryMethods.DataInfluencedBy(PDO);

// 2.2 - Implicit DB function names influenced by PDO
CxList ExecuteMethods = methods.FindByShortName("execute");
CxList ExecutePDOStmt = ExecuteMethods.InfluencedBy(PreparePDO);

// 2.3 - Implicit DB functions names related to PDOStatement
CxList FetchMethods = 
	methods.FindByShortName("fetch") +
	methods.FindByShortName("fetchAll") +
	methods.FindByShortName("fetchColumn") +
	methods.FindByShortName("fetchObject");

CxList PDOStmtMethods = FetchMethods.InfluencedBy(PreparePDO);

result.Add(QueryPDO + PDOStmtMethods);