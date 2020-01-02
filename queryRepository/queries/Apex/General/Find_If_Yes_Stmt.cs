CxList apexFiles = Find_Apex_Files() - Find_Triggers_Code() - Find_Test_Code();

result = apexFiles.GetAncOfType(typeof(IfStmt)) - Find_If_Not_Stmt();