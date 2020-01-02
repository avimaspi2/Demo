CxList methods = Find_Methods();
//Functions that test if the parameter is an integer
CxList integersFunction = methods.FindByShortNames(new List<string>{"isInteger", "isNaN", "isFinite", "isSafeInteger"});

//Get integers functions parameters
CxList integersFunctionParameters = All.GetParameters(integersFunction);
//isFinite(object.value) only keep value where FindAllReferences fails to return the correct value references
CxList values = integersFunctionParameters.FindByType(typeof(MemberAccess));
integersFunctionParameters -= values;
CxList valuesMember = values.GetTargetOfMembers();
CxList relevantValues = All.FindAllReferences(valuesMember).GetMembersOfTarget().FindByShortName(values); 

//Add all the references of the parameters of the integers functions as sanitizers
result.Add(All.FindAllReferences(integersFunctionParameters));
result.Add(relevantValues);
CxList methodsAndMembersAccess = All.NewCxList();
methodsAndMembersAccess.Add(methods);
methodsAndMembersAccess.Add(Find_UnknownReference().GetMembersOfTarget());

CxList math = methodsAndMembersAccess.FindByMemberAccess("Math.*");

// Built in math must be only one member access level
CxList notBuiltInMath = math.GetTargetOfMembers().GetTargetOfMembers().GetMembersOfTarget().GetMembersOfTarget();
result.Add(math - notBuiltInMath);
result.Add(methodsAndMembersAccess.FindByMemberAccess("*.length"));
result.Add(methodsAndMembersAccess.FindByMemberAccess("*.indexOf"));
result.Add(methodsAndMembersAccess.FindByMemberAccess("*.lastIndexOf"));
result.Add(methodsAndMembersAccess.FindByShortNames(new List<String>{ "Number", "parseInt", "parseFloat" }));

// unary operators ! !! ~ ~~ ++ //
result.Add(Find_Unarys());

// all binary operators & && | || << >> > < == === >= <= != !== * / - %
// filters are: binary operators - / % * | & ^ >> << != == >= <= > < || &&
CxList cleanExpressions = All.NewCxList();
CxList binExpressions = Find_Binarys();
foreach (CxList exp in binExpressions)
{
	BinaryExpr bin = exp.TryGetCSharpGraph<BinaryExpr>();
	if (( bin.Operator == BinaryOperator.Subtract )
		|| ( bin.Operator == BinaryOperator.Divide )
		|| ( bin.Operator == BinaryOperator.Modulus )
		|| ( bin.Operator == BinaryOperator.Multiply )
		|| ( bin.Operator == BinaryOperator.BitwiseOr )
		|| ( bin.Operator == BinaryOperator.BitwiseAnd )
		|| ( bin.Operator == BinaryOperator.BitwiseXor )
		|| ( bin.Operator == BinaryOperator.ShiftRight )
		|| ( bin.Operator == BinaryOperator.ShiftLeft )
		|| ( bin.Operator == BinaryOperator.IdentityInequality )
		|| ( bin.Operator == BinaryOperator.IdentityEquality )
		|| ( bin.Operator == BinaryOperator.GreaterThanOrEqual )
		|| ( bin.Operator == BinaryOperator.LessThanOrEqual )
		|| ( bin.Operator == BinaryOperator.GreaterThan )
		|| ( bin.Operator == BinaryOperator.LessThan ))
	{
		cleanExpressions.Add(exp);
	}
	else if( (bin.Operator == BinaryOperator.BooleanOr) || (bin.Operator == BinaryOperator.BooleanAnd )){
		
		CxList operands = All.FindByFathers(exp);
		CxList stringOperands = operands.FindByType(typeof(StringLiteral));
		CxList absStringOperands = operands.FindByAbstractValue(a => a is StringAbstractValue);
		if(absStringOperands.Count + stringOperands.Count == 0){
			cleanExpressions.Add(exp);
		}
	}
}
result.Add(cleanExpressions);

// Support Array.prototype.includes
CxList arrayCreateExpr = Find_ArrayCreateExpr();
arrayCreateExpr.Add(All.FindAllReferences(arrayCreateExpr.GetAssignee()));
CxList methodsArray = arrayCreateExpr.GetMembersOfTarget();

CxList findIncludesMethods = methods.FindByShortName("includes");
CxList findCalls = Find_Members("*.call");
CxList findIncludesProperties = findCalls.GetTargetOfMembers() * methodsArray;

result.Add(findIncludesMethods * methodsArray);
result.Add(findIncludesProperties);