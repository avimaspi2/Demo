CxList escapedOutputs = All.FindByType(typeof(ViewEscapedOutputStmt));

CxList outputs = All.NewCxList();

foreach(CxList escapedOutput in escapedOutputs){
	ViewEscapedOutputStmt view = escapedOutput.TryGetCSharpGraph<ViewEscapedOutputStmt>();	
	outputs.Add(view.Expression.NodeId, view.Expression);
}

result = outputs;