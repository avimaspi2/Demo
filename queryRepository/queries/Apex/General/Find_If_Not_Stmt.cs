CxList apexFiles = Find_Apex_Files() - Find_Triggers_Code() - Find_Test_Code();

result = apexFiles.FindByType(typeof(UnaryExpr)).FindByName("Not").GetAncOfType(typeof(IfStmt));