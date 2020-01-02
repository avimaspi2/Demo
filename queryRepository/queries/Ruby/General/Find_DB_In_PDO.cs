CxList methods = Find_Methods();

// 1 - Explicite DB function names

// 2.1 - Implicit DB function names influenced by PDO
CxList PDO = All.FindByShortName("PDO");
CxList QueryMethods = methods.FindByShortName("query");
CxList PrepareMethods = methods.FindByShortName("prepare");
CxList ExecMethods = methods.FindByShortName("exec");
CxList QueryPDO = QueryMethods.DataInfluencedBy(PDO);
CxList PreparePDO = PrepareMethods.DataInfluencedBy(PDO);
CxList ExecPDO = ExecMethods.DataInfluencedBy(PDO);

// 2.2 - Implicit DB function names influenced by PDO
CxList ExecuteMethods = methods.FindByShortName("execute");
CxList ExecutePDOStmt = ExecuteMethods.InfluencedBy(PreparePDO);

result.Add(QueryPDO + PreparePDO + ExecPDO + ExecutePDOStmt);