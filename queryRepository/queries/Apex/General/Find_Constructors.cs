CxList apexFiles = Find_Apex_Files() - Find_Triggers_Code() - Find_Test_Code();
result = apexFiles.FindByType(typeof(ConstructorDecl));

result.Add(apexFiles.FindByShortName("*__Cx_Constructor"));