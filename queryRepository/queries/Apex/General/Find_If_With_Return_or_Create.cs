CxList apexFiles = Find_Apex_Files() - Find_Triggers_Code() - Find_Test_Code();
CxList returnInsideIfStmt = apexFiles.FindByType(typeof(ReturnStmt)).GetAncOfType(typeof(IfStmt));
CxList createInsideIfStmt = apexFiles.FindByType(typeof(ObjectCreateExpr)).GetAncOfType(typeof(IfStmt));

result = returnInsideIfStmt + createInsideIfStmt;