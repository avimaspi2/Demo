// The Collections.sort() and Collections.unique() methods mutates the list and returns the list as a value. 
// If we assign the result of sort() or unique() to a variable, then we are also modifying the original list as well.
CxList method_calls = Find_Methods();
CxList filtered_method_calls = method_calls.FindByName("*.sort") +
	method_calls.FindByName("*.unique");

result = filtered_method_calls.GetAncOfType(typeof(FieldDecl)) + 
	filtered_method_calls.GetAncOfType(typeof(AssignExpr)) +
	filtered_method_calls.GetAncOfType(typeof(VariableDecl));