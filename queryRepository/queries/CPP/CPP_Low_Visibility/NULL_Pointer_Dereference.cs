CxList pointers = Find_Pointers();
CxList unary = Find_Unarys();
CxList pointerTotarget = pointers.FindByFathers(unary.FindByShortName("Pointer"));

// Remove pointers that check pointer condition
CxList findConditions = Find_Conditions();
CxList pointerCondition = pointers.GetByAncs(findConditions);
CxList ifs = pointerCondition.GetFathers().FindByType(typeof(IfStmt));
pointerTotarget -= pointerTotarget.GetByAncs(ifs);

// Remove pointers used as indexerRefs
CxList indexerRefs = Find_IndexerRefs();
pointerTotarget -= pointerTotarget.GetByAncs(indexerRefs);

//////////////////////// Null Values ////////////////////////
CxList nullValues = All.NewCxList();
nullValues.Add(Find_NullLiteral());
nullValues.Add(Find_CharLiteral().FindByName("\0"));
CxList zero = Find_IntegerLiterals().FindByShortName("0");

//Remove 0 that are in IterationStmt
CxList zeroIter = zero.GetAncOfType(typeof(IterationStmt));
zero -= zero.GetByAncs(zeroIter); 

nullValues.Add(zero);

//Remove 0 that are in MethodInvokeExpr
CxList zeroMethodInvokeExpr = nullValues.GetAncOfType(typeof(Param));
nullValues -= nullValues.GetByAncs(zeroMethodInvokeExpr);

// Remove ArrayInitializers
nullValues -= nullValues.FindByFathers(nullValues.GetFathers().FindByType(typeof(ArrayInitializer)));

// Remove null values assigned to Member Access
CxList nullValuesAssigned = nullValues.GetAssignee();
nullValues -= nullValuesAssigned.FindByType(typeof(MemberAccess)).GetAssigner();

// Remove all assignees of null values from pointers 
pointerTotarget -= pointerTotarget.GetByAncs(nullValuesAssigned);

//////////////////////// Influencing ////////////////////////
// Remove the 0 or 1  ( return 0  or return 1 )
CxList returnStmts = Find_ReturnStmt();
CxList removeZeroInReturn = All.NewCxList();
removeZeroInReturn.Add(All.GetByAncs(returnStmts));
removeZeroInReturn -= returnStmts;
CxList influencing = nullValues - removeZeroInReturn;

// Remove the 0 that are in conditions (if, while, for, etc...)
influencing -= influencing.GetByAncs(findConditions);

// Remove the 0 that are in an IndexerRef
CxList zeroIndexerRef = nullValues.GetAncOfType(typeof(IndexerRef));
influencing -= nullValues.GetByAncs(zeroIndexerRef);

//////////////////////// Sanitizers ////////////////////////
CxList sanitizers = All.NewCxList();

// Sanitize Conditions
sanitizers.Add(pointerCondition);

// Sanitize &*
CxList address = unary.FindByShortName("Address");
CxList pointerUnary = unary.FindByShortName("Pointer");
CxList addressChild = pointerUnary.FindByFathers(address);
sanitizers.Add(pointers.FindByFathers(addressChild));

// Sanitize address passed as parameter
CxList parameters = Find_Param();
sanitizers.Add(pointers.FindByFathers(address.FindByFathers(parameters)));

// Sanitize others
CxList binaryExpr = Find_BinaryExpr();
sanitizers.Add(binaryExpr);

// Remove pointer dereferences appearing in if blocks that are not executed (ie if (x != null)// when x is null)
sanitizers.Add(pointerTotarget.GetByAncs(Find_Ifs_NullCondition()));

//////////////////////// Results ////////////////////////
result = pointerTotarget.InfluencedByAndNotSanitized(influencing, sanitizers);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

//////////////////////// Pointer member access without memory allocation ////////////////////////
CxList pointerMemberAccess = pointers.GetTargetsWithMembers();
CxList binaryExprEqual = binaryExpr.FindByShortName("==");
CxList methods = Find_Methods();
CxList forEach = Find_ForEachStmt();
CxList parametersAddress = address.GetByAncs(parameters);
CxList parametersAddressPointer = pointers.GetByAncs(parametersAddress);

foreach (CxList memberAccess in pointerMemberAccess) {
	CxList pointerDefinition = pointers.FindDefinition(memberAccess).FindByType(typeof(Declarator));
	pointerDefinition -= pointerDefinition.GetByAncs(forEach);
	CxList definitionScope = methods.GetMethod(pointerDefinition);
	CxList memberAccessScope = methods.GetMethod(memberAccess);
	CxList pointerReferences = pointers.FindAllReferences(pointerDefinition) - pointerMemberAccess;
	CxList pointerReferencesScope = methods.GetMethod(pointerReferences);
	CxList scope = memberAccessScope * definitionScope * pointerReferencesScope;
	
	// All in same scope?
	if (scope.Count == 1) {
		// get declaration initialization
		CxList pointerAssign = All.FindInitialization(pointerDefinition);
		// get all references assignees
		pointerAssign.Add(pointerReferences.GetAssigner());
		// get address of pointer being passed as a parameter
		pointerAssign.Add(pointerReferences * parametersAddressPointer);
		// remove null values
		pointerAssign -= nullValues;
		// check if there is an if statement between declaration and dereference of pointer to validate pointer
		// Example: if (pointer == NULL) return ;
		CxList allUnderDefinitionScope = All.GetByAncs(definitionScope);
		CxList conditionsInScope = findConditions * allUnderDefinitionScope;
		CxList comparesInScope = binaryExprEqual * allUnderDefinitionScope;
		foreach (CxList compare in comparesInScope) {
			CxList left = pointerReferences.FindByFathers(compare);
			CxList right = nullValues.FindByFathers(compare);
			CxList childReturnStmts = returnStmts.FindByFathers(compare);
			if (left.Count == 1 && right.Count == 1)
				if (childReturnStmts.Count == 1)
					pointerAssign.Add(compare);
		}
		
		// Remove pointer accesses inside if checks (ex if(x != null) x->y = 3; )
		CxList possibleNotNullBinary = binaryExpr.GetByBinaryOperator(BinaryOperator.IdentityInequality);
		possibleNotNullBinary = possibleNotNullBinary * allUnderDefinitionScope;
		CxList toRemove = All.NewCxList();
		foreach(CxList val in possibleNotNullBinary) {
			CxList left = pointerReferences.FindByFathers(val);
			CxList right = nullValues.FindByFathers(val);
			if (left.Count >= 1 && right.Count >= 1)
				toRemove.Add(val);
		}

		// Gather cases where "!= null" is implicit (if (x) x->a)
		CxList impNull = pointerReferences.GetFathers() * ifs;
		CxList notNullAndPointerChecks = toRemove;
		CxList ifNotNullChecks = (notNullAndPointerChecks.GetFathers() * Find_Ifs()) + impNull;
		CxList unsanMembers = memberAccess - memberAccess.GetByAncs(ifNotNullChecks.GetBlocksOfIfStatements(true));

		if (pointerAssign.FindInScope(pointerDefinition, unsanMembers).Count == 0 && unsanMembers.Count > 0) {
			result.Add(pointerDefinition.Concatenate(unsanMembers));
		}
	}
}