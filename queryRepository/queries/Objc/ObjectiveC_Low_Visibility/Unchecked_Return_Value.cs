CxList conditions = Get_Conditions(); // Returns the elements inside conditions
CxList methodsWithReturnValue = Find_Methods_With_Return_Value();

CxList allAllocations = All.NewCxList();
allAllocations.Add(methodsWithReturnValue);
allAllocations.Add(methodsWithReturnValue.GetAncOfType(typeof(CastExpr)));

//Get get methods with no return value assignements
CxList allocationsAssignees = (allAllocations).GetAssignee();
CxList allReferences = All.FindAllReferences(allocationsAssignees);

//Find unkownReferences in if's, it will help to find the following case void* arg_buffer = malloc(arg_size); if (arg_buffer) {}
CxList unknownReferenceMemberAccess = conditions.FindByType(typeof(UnknownReference));
unknownReferenceMemberAccess.Add(conditions.FindByType(typeof(MemberAccess)));
CxList unknownReferencesInIf = unknownReferenceMemberAccess.GetByAncs(conditions);

//All allocations that are being verified
CxList allAllocationsInConditions = unknownReferencesInIf * allReferences;
//Remove the ones that are beign verified 
allReferences -= All.FindAllReferences(allAllocationsInConditions);
//Get the place were the allocations that are not being verified are assigned
CxList allAllocationsNotInConditions = allReferences * allocationsAssignees;
//Get the method with no return value that is not being verified
CxList tempResult = allAllocationsNotInConditions.GetAssigner() * allAllocations;

//Builds the flow of the returned result to the assigned variable
foreach (CxList elem in tempResult)
{
	result.Add(elem.ConcatenatePath(elem.GetAssignee(), false));
}

//Case the function is being used without its returning value being assigned to a variable
CxList declaratorAssigns = Find_Declarators();
declaratorAssigns.Add(Find_AssignExpr());	
//Get the methods that are assigned to a variable
CxList methodsAssign = allAllocations.GetByAncs(declaratorAssigns);
//Keep only the methods that are not being assigned to a variable
CxList notAssignedMethods = allAllocations - methodsAssign;
//Get the methods that are being used inside a condition
CxList notAssignedMethodsInCondition = notAssignedMethods.GetByAncs(conditions);
//Remove the methods that are being verified
CxList notAssignedMethodsNotInCondition = notAssignedMethods - notAssignedMethodsInCondition;
result.Add(notAssignedMethodsNotInCondition);