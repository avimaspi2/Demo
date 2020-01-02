CxList apexFiles = Find_Apex_Files() - Find_Triggers_Code() - Find_Test_Code();
CxList constructor = Find_Constructors();

result = apexFiles.FindByType(typeof(ObjectCreateExpr)).GetByAncs(constructor);