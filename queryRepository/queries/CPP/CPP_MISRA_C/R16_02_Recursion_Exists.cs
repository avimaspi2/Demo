/*
MISRA C RULE 16-2
------------------------------
This query searches for functions which directly or indirectly call themselves:

	The Example below shows code with vulnerability: 

int func(int a){
    return func(a);
}

*/

CxList methods = Find_Methods();
CxList methodDecls = All.FindByType(typeof(MethodDecl));

//remove methods which are not called or not defined inside the scanned project. 
methodDecls = methodDecls.FindDefinition(methods);
methods = methods.FindAllReferences(methodDecls);

foreach (CxList cur in methods)
{
	
	CxList curDef = methodDecls.FindDefinition(cur);
	//handle simple recursions: functions which call themselves directly
	if (cur.GetByAncs(curDef).Count > 0)
	{
		result.Add(cur);
	}
	else
	{
		//handle complex 2nd order recursions: functions which call themselves indirectly (through another function)
		CxList fatherMethodDef = cur.GetAncOfType(typeof(MethodDecl)) - curDef;
		CxList methodsInCurDefBody = methods.GetByAncs(curDef) - cur;
		CxList refMethods = methodsInCurDefBody.FindAllReferences(fatherMethodDef);
		if (refMethods.Count > 0)
		{
			result.Add(cur);
		}
	}		
}