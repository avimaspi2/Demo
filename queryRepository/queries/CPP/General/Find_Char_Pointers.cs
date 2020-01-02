CxList chars = All.FindByName("char") + Find_VariableDeclStmt();
chars.Add(Find_ESQL_Char().FindByType(typeof(UnknownReference)));
CxList pointers = (All-chars).FindByRegex(@"(char\s|\,)\s*\*+\s*\w*");
CxList methodDecl = All.FindByType(typeof(MethodDecl));
CxList pointersAsParameters = pointers.GetParameters(methodDecl);
CxList pointersAsDeclarators = pointers.FindByType(typeof(Declarator));

CxList allPointers = pointersAsParameters + pointersAsDeclarators;

allPointers = allPointers.FindDefinition(allPointers);
result = All.FindAllReferences(allPointers) - All.GetByAncs(Find_IndexerRefs());