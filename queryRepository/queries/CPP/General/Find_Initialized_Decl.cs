// Find Initialized Declarators
CxList declarators = Find_All_Declarators();
CxList references = Find_Reference() + Find_PrimitiveExpr();

//result = declarators.FindByInitialization(references);
result = declarators.FindByRegex(@"=");