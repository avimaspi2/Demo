CxList outputs = Find_Web_Outputs();
if (outputs.Count > 0)
{
	CSharpGraph graph = outputs.GetFirstGraph();
	if (graph != null)
	{
		result = All.FindById(graph.NodeId);
	}
}