//Findind all methods, classes and strings
CxList methods = Find_Methods();
CxList classDecl = Find_Class_Decl();
CxList stringTypes = Find_String_Types();
CxList classTypes = All.FindByType("Class");
//Aux list to later use to find all methods that return the name of the class
List<string> getNameMethods = new List<string> {"getName","getSimpleName"};
//All comparison methods
CxList comparisonMethods = methods.FindByShortName("equals");
CxList stringComparisonMethods = comparisonMethods.GetTargetOfMembers() * stringTypes;
stringComparisonMethods = stringComparisonMethods.GetMembersOfTarget();
//We look for the variables to find the assignees. Is more eficient then using "All"
CxList variables = Find_UnknownReference();
variables.Add(Find_Declarators());
//We filter our results by looking only for strings or classes
CxList toRemove = All.NewCxList();
toRemove.Add(stringTypes);
toRemove.Add(classTypes);
toRemove.Add(All.FindByMemberAccess("*.class").GetTargetOfMembers());

CxList sanitizers = variables - toRemove;

//Empty list to store the suspects that are found in the foreach
CxList suspectClass = All.NewCxList();
CxList suspectClassName = All.NewCxList();
CxList suspectComparison = All.NewCxList();

//Find all getClass methods and for each of those methods, see if they are used in a comparison
//This prevents wrong flow connections between Classes and comparisons
CxList getClassMethods = methods.FindByShortName("getClass");
getClassMethods.Add(classTypes);
foreach(CxList getClass in getClassMethods) {
	//We need, not only the getClass methods, but also variables to wich the class was assigned to
	CxList allClasses = variables.FindAllReferences(getClass.GetAssignee(variables));
	allClasses.Add(getClass);
	
	//We need the getName methods that have a direct connection with the getClass
	CxList getNames = allClasses.GetMembersOfTarget().FindByShortNames(getNameMethods);
	getNames.Add(allClasses.FindByType(typeof(IndexerRef)).GetFathers());
	getNames.Add(All.FindAllReferences(getNames.GetAssignee(variables)));
	
	//Special case scenario that finds methods that return the name of a class
	CxList suspectMethodDecls = getNames.GetAncOfType(typeof(MethodDecl));
	CxList suspectMethodInvokes = methods.FindAllReferences(suspectMethodDecls);	
	//Follow the same logic as before for this special case scenario
	CxList getMethodNames = suspectMethodInvokes.GetMembersOfTarget() * comparisonMethods;
	getNames.Add(getMethodNames);
	
	//Finds comparisons done after the get name (i.e. Class.GetName().Equals(Something))
	CxList getEquals = getNames.GetMembersOfTarget() * comparisonMethods;
	 
	//Finds comparisons done before the get name (i.e. Something.Equals(Class.GetName()))
	getEquals.Add(getNames.GetAncOfType(typeof(MethodInvokeExpr)) * comparisonMethods);
	getEquals.Add(getNames.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)) * comparisonMethods);
	//Makes sure we find all comparisons within the same class
	getEquals.Add(stringComparisonMethods.GetByAncs(classDecl.GetClass(getNames)));
	
	//We add our supects to their respective lists
	suspectClass.Add(getClass);
	suspectClassName.Add(getNames);
	suspectComparison.Add(getEquals);
}

//We add the result if there is a comparison used on a getName method influenced by the getClass method
result = suspectClassName.InfluencedByAndNotSanitized(suspectClass, sanitizers);
//The resulting flow must go from Class to equals unsuring it contains the class name
result = suspectComparison.InfluencedByAndNotSanitized(result, sanitizers, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);