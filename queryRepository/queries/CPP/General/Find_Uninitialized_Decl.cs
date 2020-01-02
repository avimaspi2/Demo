// Find Initialized Declarators
CxList declarators = Find_All_Declarators();
result = declarators - Find_Initialized_Decl();