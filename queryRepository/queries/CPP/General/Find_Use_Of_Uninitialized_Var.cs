CxList unkRefs = Find_Unknown_References();
CxList uninitialized_vars = unkRefs.FindByAbstractValue(x => x is NoneAbstractValue || x is AnyAbstractValue || x is ObjectAbstractValue);

CxList declarators = Find_Declarators();
CxList builtinTypes = Find_Builtin_Types();
CxList pointerDecl = Find_Pointer_Decl();
CxList nullLiterals = Find_NullLiteral();
CxList unkAndDecl = unkRefs;
unkAndDecl.Add(declarators);

//we don't need to take into account static variables nor extern variables
CxList decls = declarators.GetAncOfType(typeof(VariableDeclStmt));
decls.Add(declarators.GetAncOfType(typeof(FieldDecl)));
decls.Add(declarators.GetAncOfType(typeof(ConstantDeclStmt)));

CxList staticMods = decls.FindByFieldAttributes(Modifiers.Static);
CxList externMods = decls.FindByFieldAttributes(Modifiers.Extern);

declarators -= declarators.GetByAncs(staticMods);
declarators -= declarators.GetByAncs(externMods);

// we will remove the fields that are neither builtin types nor pointer types.
// the others are considered to be implicitly initialized by compiler
CxList fieldDecls = declarators.GetAncOfType(typeof(FieldDecl));
CxList restrictedFields = declarators.GetByAncs(fieldDecls);
CxList fieldDeclsAsBuiltinOrPointer = (restrictedFields * builtinTypes);
fieldDeclsAsBuiltinOrPointer.Add(restrictedFields * pointerDecl);
CxList nonBuiltinOrPointerFields = restrictedFields - fieldDeclsAsBuiltinOrPointer;
declarators -= nonBuiltinOrPointerFields;

// remove the ones in the left side of an assignment
CxList nulls = nullLiterals.GetAssignee();
CxList unkAndDeclAndInd = unkAndDecl.GetAncOfType(typeof(IndexerRef));
unkAndDeclAndInd.Add(unkAndDecl);
CxList left = unkAndDeclAndInd.FindByAssignmentSide(CxList.AssignmentSide.Left);
left.Add(nulls);
CxList allLeft = unkAndDecl.FindAllReferences(left);
uninitialized_vars -= allLeft;

CxList unaryExpr = Find_Unarys();
CxList addresses = unaryExpr.FindByShortName("Address");
CxList pointers = unaryExpr.FindByShortName("Pointer");
CxList sanitize = uninitialized_vars.FindByFathers(addresses);
uninitialized_vars -= sanitize;
uninitialized_vars -= uninitialized_vars.FindByFathers(pointers);

// remove the variables defined in the Catch (they are implicitly initialized)
CxList catchs = Find_Catch();
uninitialized_vars -= uninitialized_vars.GetByAncs(catchs);

//remove variables in foreach: ex: for(auto k : list) -> removes k
CxList foreachs = Find_ForEachStmt();
CxList variables = Find_VariableDeclStmt();
CxList variableForEach = variables.FindByFathers(foreachs);
CxList forEachVariable = declarators.GetByAncs(variableForEach);
CxList forEachVariableAllRefs = unkAndDecl.FindAllReferences(forEachVariable);
uninitialized_vars -= forEachVariableAllRefs;

CxList initialized = All.NewCxList();
CxList allStreams = unkRefs.FindByType("istringstream");
allStreams.Add(unkRefs.FindByType("istream"));
allStreams.Add(unkRefs.FindByType("ifstream"));
CxList istrStream = allStreams.GetFathers().FindByType(typeof(BinaryExpr)).GetByBinaryOperator(BinaryOperator.ShiftRight);
foreach(CxList issm in istrStream)
{
	BinaryExpr be = issm.TryGetCSharpGraph<BinaryExpr>();
	if(be != null)
	{
		initialized.Add(be.Right.NodeId, be.Right);
	}
}

CxList allRefsInitialized = unkAndDecl.FindAllReferences(initialized);
uninitialized_vars -= allRefsInitialized;

sanitize.Add(Find_Variables_Passed_ByRef());
uninitialized_vars -= sanitize;
	
foreach(CxList v in uninitialized_vars)
{
	CxList definition = declarators.FindDefinition(v);
	if (definition.Count > 0 && definition.FindByType(typeof(ParamDecl)).Count == 0)
	{
		CxList scope = sanitize.FindByShortName(v);
		if (scope.FindInScope(definition, v).Count == 0)
		{
			result.Add(definition.ConcatenatePath(v,false));
		}
	}
}