CxList viewInputs = All.FindByType(typeof(ViewInputStmt));

CxList inputs = All.NewCxList();

foreach(CxList viewInput in viewInputs){
	ViewInputStmt view = viewInput.TryGetCSharpGraph<ViewInputStmt>();	
	inputs.Add(view.Expression.NodeId, view.Expression);
}

result = inputs;