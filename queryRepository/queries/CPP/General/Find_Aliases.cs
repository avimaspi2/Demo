// Find All references and aliases of an array
// e.g., in the following code buf_alias is an alias of buf.
//	char buf[5];
//	char* buf_alias = buf;
//
// Another kind of alias is by when the array is sent as a parameter.
//	

if(param.Length == 1)
{
	CxList array = param[0] as CxList;
	if(array != null)
	{
		// Find aliases by assignment
		CxList arrayCreateRef = All.FindAllReferences(array).FindByAssignmentSide(CxList.AssignmentSide.Right);
		CxList arrayAlias = arrayCreateRef.GetFathers().FindByType(typeof(AssignExpr));
		arrayAlias = All.FindByAssignmentSide(CxList.AssignmentSide.Left).FindByType(typeof(UnknownReference)).GetByAncs(arrayAlias);
		arrayAlias = All.FindAllReferences(arrayAlias).FindByType(typeof(UnknownReference));
		result = All.FindAllReferences(array);
		result.Add(arrayAlias);			
		
		// Find aliases by sending as parameters
		CxList methodDefinitions = Find_Method_Declarations();
		CxList AllParamsDecls = Find_ParamDecl();
		CxList parameters = result.GetFathers().FindByType(typeof(Param));
		foreach(CxList p in parameters)
		{
			int paramIndex = p.GetIndexOfParameter();
			CxList methodInvoke = Find_Methods().FindByParameters(p);
			CxList methodDefine = methodDefinitions.FindDefinition(methodInvoke);
			CxList paramAlias = AllParamsDecls.GetParameters(methodDefine, paramIndex);
			result.Add(All.FindAllReferences(paramAlias));	
		}
	}
}