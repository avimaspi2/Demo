CxList viewOutputs = All.FindByType(typeof(ViewOutputStmt));

CxList outputs = All.NewCxList();

foreach(CxList viewOutput in viewOutputs){
	ViewOutputStmt view = viewOutput.TryGetCSharpGraph<ViewOutputStmt>();	
	outputs.Add(view.Expression.NodeId, view.Expression);
}

result = outputs;