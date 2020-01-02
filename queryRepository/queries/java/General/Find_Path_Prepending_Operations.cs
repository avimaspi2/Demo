/*
	Finds all the right side expressions of a plus binary expression.
	Also returns the arguments of path building operations, except for the first one.
		Ex: Paths.get("C:", "Users", "file.txt");
	Find_Path_Prepending_Sanitizers() will return "Users" and "file.txt" in this case.
*/

CxList methods = Find_Methods();
CxList binaryExpressions = base.Find_BinaryExpr();

/*
	Get the right side expression of additions
*/
CxList RightPlusExpression = All.NewCxList();
foreach(CxList binaryExpression in binaryExpressions){
	BinaryExpr binaryExpr = binaryExpression.TryGetCSharpGraph<BinaryExpr>();
	if(binaryExpr.Operator == BinaryOperator.Add){
		RightPlusExpression.Add(All.FindById(binaryExpr.Right.NodeId));
	}
}

result = RightPlusExpression;

/*
	Add other string appending operations parameters 
*/
CxList appendingOps = methods.FindByMemberAccess("StringBuilder.append");
appendingOps.Add(methods.FindByMemberAccess("StringBuffer.append"));
appendingOps.Add(methods.FindByShortName("concat"));
result.Add(All.GetParameters(appendingOps,0));

/* 	
	Both Paths.get()and FileSystem.getPath() receive multiple arguments and return a Path resulting 
	of the composition of those arguments. 

		Ex: Paths.get("C:","Users","file.txt"); results in the path "C:\Users\file.txt", 
		same as FileSystem.getPath("C:","Users","file.txt");

	All arguments except for the first one, are prepended something(the value of the first parameter).
*/
CxList pathsGetMethods = methods.FindByMemberAccess("Paths.get");
pathsGetMethods.Add(methods.FindByShortName("getPath"));
CxList pathsGetMethodsParameters = All.GetParameters(pathsGetMethods);
CxList pathsGetMethodsFirstParameter = pathsGetMethodsParameters.GetParameters(pathsGetMethods, 0);
result.Add(pathsGetMethodsParameters - pathsGetMethodsFirstParameter);

/*
	The File object constructor can receive one or two argumens. If two are given,
	the created file is a product of the path built from the sum of the two.
	Ex: new File("C:","Users\\file.txt"); will result in "C:\Users\file.txt".
	Therefore, these second parameters are appendings as well.
*/
CxList fileOpen = Find_Files_Open();
result.Add(All.GetParameters(fileOpen.FindByShortName("File"),1));
/*
	File.createTempFile can either receive 2 or 3 arguments. Ex:
		createTempFile("/tmpFiles/", "file.txt");
		createTempFile("file",".txt",new File("/tmpFiles"));
*/
CxList tmpFileParams = All.GetParameters(fileOpen.FindByShortName("createTempFile"));
result.Add(tmpFileParams.GetParameters(fileOpen, 1));
CxList tempFileWithThreeArgs = fileOpen.FindByParameters(tmpFileParams.GetParameters(fileOpen, 2));
result.Add(tmpFileParams.GetParameters(tempFileWithThreeArgs, 0));
result.Add(tmpFileParams.GetParameters(tempFileWithThreeArgs, 1));
/*
	Workaround due to the lack of flow between some indexer references and their unknown references.
*/
result.Add(All.FindByFathers(result.FindByType(typeof(IndexerRef))));