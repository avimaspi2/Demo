CxList methods = Find_Methods();

// 2.1 - Implicit DB function names influenced by PDO
CxList PDO = All.FindByShortName("PDO");
CxList PrepareMethods = methods.FindByShortName("prepare");
CxList PreparePDO = PrepareMethods.DataInfluencedBy(PDO);

// 2.2 - Implicit DB function names influenced by PDO
CxList ExecuteMethods = methods.FindByShortName("execute");
CxList ExecutePDOStmt = ExecuteMethods.InfluencedBy(PreparePDO);

result.Add(ExecutePDOStmt);