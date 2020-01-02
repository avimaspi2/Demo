result = All.FindByType(typeof(ArrayCreateExpr));

//Arrays that are function parameters
CxList paramDecls = Find_ParamDecl();
foreach(CxList iteration in paramDecls){
	try 
	{
		ParamDecl decl = iteration.TryGetCSharpGraph<ParamDecl>();
		if(decl.Type.ArrayRanks.Count > 0){
			result.Add(iteration);
		}
	} catch(Exception) { }
}

//Array of chars - FieldDecls
CxList fieldDecls = Find_FieldDecls();
foreach(CxList iteration in fieldDecls){
	try 
	{
		FieldDecl decl = iteration.TryGetCSharpGraph<FieldDecl>();
		if(decl.Type.ArrayRanks.Count > 0 && result.GetByAncs(iteration).Count == 0) {
			result.Add(iteration);
		}
	} catch(Exception) { }
}

//Array of chars - VarDecls
CxList varDecl = Find_VariableDeclStmt();
foreach(CxList iteration in varDecl){
	try {
		VariableDeclStmt decl = iteration.TryGetCSharpGraph<VariableDeclStmt>();
		if(decl.Type.ArrayRanks.Count > 0 && result.GetByAncs(iteration).Count == 0) {
			result.Add(iteration);
		}
	} catch(Exception) { }
}

//Array of chars - ConstDecls
CxList constDecl = Find_ConstantDecl();
foreach(CxList iteration in constDecl){
	try 
	{
		ConstantDecl decl = iteration.TryGetCSharpGraph<ConstantDecl>();
		if(decl.Type.ArrayRanks.Count > 0 && result.GetByAncs(iteration).Count == 0) {
			result.Add(iteration);
		}
	} catch(Exception) { }
}