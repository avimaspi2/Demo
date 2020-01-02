// Find Uninitialized Pointer Declarators

// Find Pointer Declarators
CxList declarators = Find_Pointer_Decl();

// // Find initialized
CxList references = Find_Reference() + Find_PrimitiveExpr()
				  + Find_Methods();
CxList initialized = declarators.FindByInitialization(references);

result = declarators - declarators.FindByRegex(@"=|\[");
result -= initialized;