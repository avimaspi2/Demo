//Find for GCC de-optimization only with O0 (sanitizes all sensitive data)
CxList pragmaDirectives_O0 = Find_Pragma_Directives("optimize", "\\(\"O0\"\\)");
//Pragma O0 sanitizes every scenario of Heap inspection
if(pragmaDirectives_O0.Count <= 0)
{
	CxList methods = Find_Methods();
	CxList declarators = Find_Declarators();
	CxList unknowRefs = Find_Unknown_References();
	CxList literals = Find_Strings();
	literals.Add(Find_Integer_Literals());
	CxList objectCreateExpr = Find_ObjectCreations();
	CxList membersAccess = Find_MemberAccesses();
	CxList variableDeclStat = Find_VariableDeclStmt();	

	//1) Collect sensitive data
	CxList passwords = Find_All_Passwords();
	string[] safeTypesArray = { "*.SecureString","SecureString","*.cryptostream","cryptostream","IntegerLiteral"};
	//Remove safetypes
	passwords -= declarators.FindByTypes(safeTypesArray);
	passwords -= unknowRefs.FindByTypes(safeTypesArray);
	passwords -= passwords.FindByType(typeof(ConstantDecl));

	//2) Filter passwords with sanitizers usage
	CxList encryptParams = unknowRefs.GetParameters(Find_Encrypt());
	CxList scopeEncryptSanitizers = unknowRefs;
	scopeEncryptSanitizers.Add(membersAccess);
	scopeEncryptSanitizers.Add(declarators);
	passwords -= scopeEncryptSanitizers.FindAllReferences(encryptParams);

	//Remove passwords with volatile qualifier
	CxList passwordsWithVolatileQualifier = passwords.GetByAncs(variableDeclStat.FindByFieldAttributes(Modifiers.Volatile));
	passwords -= passwords.FindAllReferences(passwordsWithVolatileQualifier);

	//List of methods that act as sanitizer
	List < string > sanitizersMethodsList = new List<string> {"memset_s", "explicit_bzero", "SecureZeroMemory"};
	CxList sanitizerMethods = methods.FindByShortNames(sanitizersMethodsList);
	CxList sanitizersParams = passwords.GetParameters(sanitizerMethods);
	CxList sanitizersParamsAllRefs = passwords.FindAllReferences(sanitizersParams);
	//Get all passwords assigned with a password that is sanitized
	CxList passwordsAssignedWithSanitizedPass = sanitizersParamsAllRefs.FindByAssignmentSide(CxList.AssignmentSide.Right).GetFathers();
	//All references of these passwords
	CxList passwordsAssignedWithSanitizedPassRefs = unknowRefs.FindAllReferences(passwordsAssignedWithSanitizedPass);
	//Allocation methods
	CxList allocationMethods = methods.FindByShortNames(new List<string>{"calloc","malloc"});
	//Assignee of allocation methods
	CxList allocatedVariables = allocationMethods.GetAssignee();
	//Get passwords assigned with sanitized passwords but allocated after
	CxList passwordsAssignedWithSanitizedPassAllocatedAfter = allocatedVariables * passwordsAssignedWithSanitizedPassRefs;
	//Remove them from the list of passwords assigned with sanitized passwords
	passwordsAssignedWithSanitizedPassRefs -= passwordsAssignedWithSanitizedPassAllocatedAfter;

	passwords -= passwordsAssignedWithSanitizedPassRefs;
	passwords -= sanitizersParamsAllRefs;


	//3) Find assignments of literals
	CxList assignLiteral = literals.GetFathers();
	//Find password that is assigned a literal
	CxList passAssigned = passwords.FindByAssignmentSide(CxList.AssignmentSide.Left);
	CxList passAssignLiteral = passAssigned.FindByFathers(assignLiteral);
	passAssignLiteral.Add(assignLiteral);
	//remove password that is assigned a literal
	passAssigned -= passAssignLiteral; 
	//remove arrayCreations
	CxList arrayCR = Find_ArrayCreateExpr().FindByAssignmentSide(CxList.AssignmentSide.Right);
	passAssigned -= arrayCR.GetFathers();
	//Remove variable declarations without assignments
	passAssigned -= All.FindByFathers(passAssigned).FindByType(typeof(ObjectCreateExpr)).GetFathers();

	//4) Find passwords that are class members
	CxList classMembers = unknowRefs.FindByPointerType("*").GetRightmostMember();
	CxList passwordsMembers = passwords * classMembers;

	//5) Find passwords that obtain a value from a gets/fgets/strcpy execution
	List<string> unsafeMethods = new List<string>{"gets", "fgets", "strcpy","realloc"};
	CxList unsafeMethodsParams = unknowRefs.GetParameters(methods.FindByShortNames(unsafeMethods));
	CxList passUsedUnsafeMethods = unsafeMethodsParams * passwords;

	CxList passwordsToCheck = passUsedUnsafeMethods;
	passwordsToCheck.Add(passwordsMembers);
	passwordsToCheck.Add(passAssigned);

	//6) Find Pragma Directives
	//Find for enabled optimization of the main function
	CxList pragmaDirectivesOptimizeOn = Find_Pragma_Directives("optimize", "\\(\"\",\\ *on\\)");
	//Find for disabled optimization of the main function
	CxList pragmaDirectivesOptimizeOff = Find_Pragma_Directives("optimize", "\\(\"\",\\ *off\\)");
	passwordsToCheck -= passwords.FindInScope(pragmaDirectivesOptimizeOff, pragmaDirectivesOptimizeOn); 

	result = declarators.FindDefinition(passwordsToCheck);
}