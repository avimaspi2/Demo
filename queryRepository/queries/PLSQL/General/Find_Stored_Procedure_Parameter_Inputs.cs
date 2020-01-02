/*
Get all public methods parameters that are input or input/output.
The output parameters shouldn't be considered.
*/

CxList method_decl = All.FindByType(typeof(MethodDecl));
method_decl -= method_decl.FindByFieldAttributes(Modifiers.Private);

CxList parameters = All.FindByType(typeof(ParamDecl));
CxList methodParameters = parameters.GetByAncs(method_decl);

CxList parametersToRemove = All.NewCxList();
foreach (CxList methodParameter in methodParameters)
{
	try
	{
		ParamDecl parameter = methodParameter.TryGetCSharpGraph<ParamDecl>();
		
		if (parameter.Direction == ParamDirection.Out)
		{
			parametersToRemove.Add(methodParameter);
		}
	}
	catch(Exception) {}
}

result = methodParameters - parametersToRemove;