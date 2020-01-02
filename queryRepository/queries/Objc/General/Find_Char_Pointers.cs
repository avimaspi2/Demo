CxList chars = All.FindByName("char");
chars.Add(Find_VariableDeclStmt());

CxList pointers = (All - chars).FindByRegex(@"(char\s|\,)\s*\*+\s*\w*");
CxList methodDecl = Find_MethodDecls();
CxList pointersAsParameters = pointers.GetParameters(methodDecl);
CxList pointersAsDeclarators = pointers * Find_Declarators();

CxList allPointers = All.NewCxList();
allPointers.Add(pointersAsParameters);
allPointers.Add(pointersAsDeclarators);

allPointers = allPointers.FindDefinition(allPointers);
result = All.FindAllReferences(allPointers) - All.GetByAncs(Find_IndexerRefs());