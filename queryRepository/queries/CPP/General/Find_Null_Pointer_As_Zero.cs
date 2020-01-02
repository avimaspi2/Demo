//This query finds integer literals of value 0 that are used as a NULL pointer.

CxList integerLiterals = Find_Integer_Literals();
CxList zeroIntegerLiterals = integerLiterals.FindByShortName("0");

CxList returnStmts = Find_ReturnStmt();
CxList returnZero = zeroIntegerLiterals.FindByFathers(returnStmts * zeroIntegerLiterals.GetFathers());

//Methods that return pointers to something may return 0, which symbolizes the null pointer.
CxList methodDeclarations = Find_Method_Declarations();
CxList declared = Find_StatementCollection().FindByFathers(methodDeclarations).GetFathers();

CxList returnTypes = Find_TypeRef().FindByFathers(declared);
CxList pointerReturnTypes = returnTypes.FindByRegex(@"(?<![(,])\s+\w+\s*\*[^();]*\(");

CxList methodsReturrningPointers = pointerReturnTypes.GetFathers();

result = zeroIntegerLiterals.GetByAncs(returnZero.GetByAncs(methodsReturrningPointers));