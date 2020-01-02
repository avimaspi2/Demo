CxList fields = Find_Field_Decl();
CxList methods = Find_MethodDeclaration();

foreach(CxList curField in fields)
{
	CSharpGraph graph = curField.TryGetCSharpGraph<CSharpGraph>();
	CxList methodWithSameName = methods.FindByName(graph.FullName);
	if(methodWithSameName.Count > 0)
	{
		result.Add(methodWithSameName.ConcatenateAllSources(curField));
	}
}