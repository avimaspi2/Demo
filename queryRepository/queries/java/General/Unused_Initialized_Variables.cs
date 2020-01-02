// Custom attributes
CxList customAttributes = Find_UnknownReference().GetByAncs(Find_CustomAttribute());

// Dead Code (and JSP code, that should be ignored as well)
CxList jsp = Find_Jsp_Code();
jsp = jsp.GetByAncs(jsp.FindByShortName("Checkmarx_Class_Init"));
CxList deadCode = Find_Dead_Code_Contents();
deadCode.Add(jsp);

CxList paramDecl = Find_ParamDeclaration();
paramDecl -= deadCode;

CxList vars = Find_Constant_And_Artificially_Variables();

// All getters/setters
CxList methodDecl = Find_MethodDeclaration();
CxList setters = methodDecl.FindByShortName("set*");
CxList getters = methodDecl.FindByShortName("get*");
getters.Add(methodDecl.FindByShortName("is*"));

// Have a list of variable-and-param decl, for the first type of unused variables
CxList varsAndParams = All.NewCxList();
varsAndParams.Add(vars);
varsAndParams.Add(paramDecl);
// All references of vars (and params)
CxList varsReferences = All.FindAllReferences(varsAndParams) - customAttributes;


// Find all the initialized parameters
CxList initialized = varsReferences.FindByAssignmentSide(CxList.AssignmentSide.Left);
//	Counts in variables initialized as null. FindByAssignmentSide(CxList.AssignmentSide.Left); is unable to find them.
CxList varRefers = (varsReferences.FindByType(typeof(Declarator)).GetAssigner() * All.FindByRegex(@"null").FindByType(typeof(NullLiteral))).GetAssignee();
initialized.Add(varRefers);

// Find all the reerences of the initialized parameters, not inlcuding:
// a. the initialized parameters themselves, and
// b. their declaration

CxList allInitVars = All - initialized;
allInitVars -= vars;

CxList initializedReferences = allInitVars.FindAllReferences(initialized);
// Find the definition/declaration of all variables that were initialized and used
CxList initializedRefDef = vars.FindDefinition(initializedReferences);
// Find the definition/declaration of all variables that were initialized (used or not used!!)
CxList initializedDef = vars.FindDefinition(initialized);

// Leave only the variables that were initialized but not used elsewhere
CxList onlyInitialized = initializedDef - initializedRefDef;
// Return the initialization and not the declaration (clearer for the reader)
onlyInitialized = initialized.FindAllReferences(onlyInitialized);

//	Removes variables within return statements, for example: return variable;
CxList returnStatements = Find_ReturnStmt();
onlyInitialized -= varsReferences.FindAllReferences(onlyInitialized.GetByAncs(returnStatements));


// Remove results in setters
CxList initSetters = setters.GetMethod(onlyInitialized);
CxList initSetterVars = onlyInitialized.GetByAncs(initSetters);
foreach (CxList setter in initSetters)
{
	CSharpGraph g = setter.TryGetCSharpGraph<CSharpGraph>();
	String setterName = g.ShortName;
	if (setterName.Length > 3)
	{
		setterName = setterName.Substring(3);
		CxList relevantVar = initSetterVars.GetByAncs(setter).FindByShortName(setterName, false);
		onlyInitialized -= relevantVar;
	}
}
result = onlyInitialized;