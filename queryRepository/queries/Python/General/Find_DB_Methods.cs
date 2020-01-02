CxList dbMethods = Find_DB_In();
dbMethods.Add(Find_DB_Out());

//Filter out all methods declared within the code itself
//Find all methods declarations
CxList methodDecls = Find_MethodDecls();

//Find referances to db methods declared within the code 
CxList methodRefs = dbMethods.FindAllReferences(methodDecls);

result = dbMethods - methodRefs; // remove references to db methods which declared within the code