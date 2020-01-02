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
CxList getters = methodDecl.FindByShortName("get*");
getters.Add(methodDecl.FindByShortName("is*"));

// Have a list of variable-and-param decl, for the first type of unused variables
CxList varsAndParams = All.NewCxList();
varsAndParams.Add(vars);
varsAndParams.Add(paramDecl);
// All references of vars (and params)
CxList varsReferences = All.FindAllReferences(varsAndParams) - customAttributes;


// Which vars and params were never used
CxList neverUsed = varsAndParams - varsAndParams.FindDefinition(varsReferences - varsAndParams);

// Remove parameters of abstract methods
neverUsed -= paramDecl.GetByAncs(paramDecl.GetFathers().GetFathers().FindByType(typeof(MethodDecl)).FindByFieldAttributes(Modifiers.Abstract));

// Remove HttpServletRequest and HttpServletResponse that are parameters
CxList response = neverUsed.FindByType("HttpServletResponse").FindByType(typeof(ParamDecl));
CxList request = neverUsed.FindByType("HttpServletRequest").FindByType(typeof(ParamDecl));
neverUsed -= response;
neverUsed -= request;
// Remove "main" parameters
neverUsed -= neverUsed.GetParameters(methodDecl.FindByShortName("main")).FindByShortName("args");
// Remove public members
neverUsed -= neverUsed.GetByAncs((neverUsed.GetAncOfType(typeof(FieldDecl)) * Find_Field_Decl()).FindByFieldAttributes(Modifiers.Public));

/*
The following part removes the parameters of empty body methods that are overriden in other classes
*/

CxList classDecl = Find_Class_Decl();
CxList neverUsedParams = neverUsed.FindByType(typeof(ParamDecl));

//Remove params of plugins (to reduce cycle iterations)
neverUsedParams -= neverUsedParams.FindByFileName("*Plugins\\Java*");

foreach (CxList par in neverUsedParams)
{
	//method with neverUsed parameter
	CxList methodWithNUP = methodDecl.FindByParameters(par);
	//class with neverUsed parameter
	CxList classWithNUP = classDecl.GetClass(methodWithNUP);
	
	// if no such class
	if (classWithNUP.Count == 0)
	{
		// if this class is interface
		classWithNUP = methodWithNUP.GetAncOfType(typeof (InterfaceDecl));
		if (classWithNUP.Count > 0)
		{
			neverUsed -= par;
		}
	}
}
result = neverUsed;