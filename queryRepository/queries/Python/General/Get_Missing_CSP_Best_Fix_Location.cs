List<string> middlewareForcsp = new List<string> {
	"MIDDLEWARE_CLASSES",
	"MIDDLEWARE"
	};
CxList dJangoConfig = Find_Django_Config();
CxList outputs = Find_Declarators() * dJangoConfig.FindByShortNames(middlewareForcsp);
if (outputs.Count > 0)
{
	CSharpGraph graph = outputs.GetFirstGraph();
	if (graph != null)
	{
		result = All.FindById(graph.NodeId);
	}
}else{
	CSharpGraph graph = dJangoConfig.GetFirstGraph();
	if (graph != null)
	{
		result = All.FindById(graph.NodeId);
	}
}

if(result.Count == 0){

	CxList webOutputs = Find_Web_Outputs();
	webOutputs.Add(Find_HTTP_Responses());
	if (webOutputs.Count > 0)
	{
		CSharpGraph graph = webOutputs.GetFirstGraph();
		if (graph != null)
		{
			result = All.FindById(graph.NodeId);
		}
	}
}