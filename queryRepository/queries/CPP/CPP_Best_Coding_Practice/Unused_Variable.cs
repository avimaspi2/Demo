// VariableNeverUsed
// -----------------
// The purpose of the query to find variables that defined or defined and initilized but never used 

// Find All declarators: it may be a local variables, global variables or structurs
CxList varsDecl = Find_All_Declarators();

//Exclude declarations with extern attribute
CxList externVars = All.FindByFieldAttributes(Modifiers.Extern);
varsDecl -= varsDecl.GetByAncs(externVars);

//exclude gotos 
CxList gotoExpr = Find_ObjectCreations().FindByShortName("goto");
varsDecl -= gotoExpr.GetAssignee(varsDecl);

// The approach :All.GetByAncs(All.FindByType(typeof(FieldDecl))) tooks al childs of FiledDeck that is global variables
// and remove it from the Decl list
// 
varsDecl -= All.GetByAncs(Find_FieldDecls());

CxList references = Find_UnknownReference();
references.Add(Find_MemberAccesses());
CxList allVariablesUsing = references.FindAllReferences(varsDecl);

CxList lambdaExpr = All.FindByType(typeof(LambdaExpr));
	
CxList allMethodsInvocations = Find_Methods().FindAllReferences(varsDecl);
allVariablesUsing.Add(allMethodsInvocations);
allVariablesUsing.Add(lambdaExpr.GetAssignee(varsDecl));

result =  varsDecl - varsDecl.FindDefinition(allVariablesUsing);