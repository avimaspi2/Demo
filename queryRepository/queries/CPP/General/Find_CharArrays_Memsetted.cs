CxList decls = Find_Declarators();
CxList charDecls = Find_TypeRef().FindByName("char").GetAncOfType(typeof(VariableDeclStmt));

// get all declarations of type char
//( there's no distinction in DOM between "char" and "char*" )
CxList charVars = decls.GetAncOfType(typeof(VariableDeclStmt)) * charDecls;
// convert VariableDeclStmt to Declarator
charVars = Find_Declarators().GetByAncs(charVars);

CxList all_methods = Find_Methods();

CxList targetsCopy = charVars.Clone();

CxList all_malloc_assigns = (all_methods.FindByShortName("malloc"));
all_malloc_assigns = all_malloc_assigns.GetByAncs(All.FindByType(typeof(AssignExpr)));
all_malloc_assigns = all_malloc_assigns.GetAncOfType(typeof(CastExpr));

CxList all_ArrayCreate = Find_ArrayCreateExpr();
// every malloc has a CastExpr associated so we'll be looking for CastExpr's
// to find variables assigned in the same statement where they're declared
CxList all_castExprs = All.FindByType(typeof(CastExpr));
// remove all variables of type char that aren't arrays and don't have malloc as initialization
foreach(CxList x in targetsCopy){
	bool isArray = all_ArrayCreate.GetByAncs(x).Count > 0;
	bool isAssignedMalloc = (all_malloc_assigns.GetAssignee() * All.FindAllReferences(x)).Count > 0;
	bool isMallocOnDecl = all_castExprs.GetByAncs(x).GetName().Equals("malloc");
	if(!(isArray | isAssignedMalloc | isMallocOnDecl ) )
		charVars -= x;
}
// at this point we can assume all variables in charVars are strings
// ( unless malloc's are used without a cast or are used to allocate only 1 byte )

CxList memsets = all_methods.FindByShortName("memset");

CxList allVars_uses = All.FindAllReferences(charVars);

result = charVars.FindDefinition(allVars_uses.GetParameters(memsets, 0));