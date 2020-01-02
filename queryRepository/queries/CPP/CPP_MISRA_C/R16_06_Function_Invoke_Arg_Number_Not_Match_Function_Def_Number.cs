/*
MISRA C RULE 16-6
------------------------------
This query searches for method invokations with different number of arguments than the definition of the method

	The Example below shows code with vulnerability: 

int foo ( void ){
...
}

extern int foo ( int param );

a = foo ( b );


*/

CxList methodInvokes = All.FindByType(typeof(MethodInvokeExpr));
CxList methodDefs = All.FindByType(typeof(MethodDecl));

// remove methods with variable number of arguments
methodDefs -= R16_01_Function_With_Variable_Number_Of_Arguments().GetAncOfType(typeof(MethodDecl));

CxList invParamaters = All.FindByType(typeof(Param)).GetParameters(methodInvokes);
CxList defParamaters = All.FindByType(typeof(ParamDecl)).GetParameters(methodDefs);

CxList tempParams = defParamaters.GetParameters(methodDefs);
// correct functions with empty param list (void)
foreach(CxList cur in (methodDefs)){
	CxList curParams = tempParams.GetParameters(cur);
	if (curParams.Count == 1){
		ParamDecl curPar = curParams.data.GetByIndex(0) as ParamDecl;
		if (curPar.Type != null && curPar.Type.TypeName != null && 
		String.Compare(curPar.Type.TypeName, "void") == 0){
			
			defParamaters -= All.FindById(curPar.NodeId);
		}	
	}
}
tempParams = defParamaters.GetParameters(methodDefs);
CxList nameOfInvokes = methodInvokes.FindByShortName(methodDefs);

CxList temp = invParamaters.FindByType(typeof(Param)).GetParameters(nameOfInvokes);

// go over all method definitions

foreach(CxList curMethodDef in methodDefs)
{
	CxList curParams = tempParams.GetParameters(curMethodDef);

	// compare to all same name invokations
	CxList sameNameInvokes = nameOfInvokes.FindByShortName(curMethodDef);
	foreach(CxList compMethodInvoke in sameNameInvokes)
	{		
		CxList compParams = temp.GetParameters(compMethodInvoke);
		// compare number of paramaters
		if (curParams.Count != compParams.Count){								
			CSharpGraph def = curMethodDef.data.GetByIndex(0) as CSharpGraph;
			CSharpGraph inv = compMethodInvoke.data.GetByIndex(0) as CSharpGraph;
			result.Add(def.NodeId, def);
			result.Add(inv.NodeId, inv);
			
		}
	}					
}