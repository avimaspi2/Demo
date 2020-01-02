CxList ifStmt = All.FindByType(typeof(IfStmt)).FindByRegex(@"if(\s)*\([^{]*;");
CxList iterationStmt = All.FindByType(typeof(IterationStmt)).FindByRegex(@"while(\s)*\([^{]*;");

result = ifStmt + iterationStmt;