if(param.Length == 1)
{
	CxList callbackVar = param[0] as CxList;
	CxList anonyVar = callbackVar.FindByShortName("anony*");
	CxList classFound=Get_Class_Of_Anonymous_Ref(anonyVar);
	CxList methodDecl = Find_MethodDecls();
	result= methodDecl.FindByShortName(classFound);
	
	//if a lambda
	callbackVar -= anonyVar;
	result.Add(methodDecl.FindDefinition(callbackVar));
	
}