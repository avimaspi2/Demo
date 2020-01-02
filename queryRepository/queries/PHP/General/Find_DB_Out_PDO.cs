CxList methods = Find_Methods();

// 1 - Implicit DB function names influenced by PDO
CxList PDO = All.FindByShortName("PDO");
CxList PrepareMethods = methods.FindByShortName("prepare");
CxList QueryMethods = methods.FindByShortName("query");
CxList PreparePDO = PrepareMethods.DataInfluencedBy(PDO);
CxList QueryPDO = QueryMethods.DataInfluencedBy(PDO);
QueryPDO.Add(All.FindByMemberAccess("PDO.query"));

// 2 - Implicit DB functions names related to PDOStatement
CxList FetchMethods = methods.FindByShortNames(new List<string> {"fetch", "fetchAll", "fetchColumn", "fetchObject"});
CxList PDOStmtMethods = FetchMethods.InfluencedBy(PreparePDO);

result.Add(QueryPDO + PDOStmtMethods);