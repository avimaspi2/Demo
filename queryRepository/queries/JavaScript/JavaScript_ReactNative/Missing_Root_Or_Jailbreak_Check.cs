bool sanitized = ReactNative_Find_Device_Root_Verification().Count > 0;

if (ReactNative_Find_Presence().Count > 0 && !sanitized)
{
	CxList methodDecls = Find_MethodDecls();
	CxList render = methodDecls.FindByShortName("render");
	
	CSharpGraph graph = render.GetFirstGraph();
	result.Add(graph.NodeId, graph);
}