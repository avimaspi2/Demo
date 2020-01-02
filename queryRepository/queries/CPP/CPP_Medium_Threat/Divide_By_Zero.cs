CxList zero = All.FindByTypes(new string[]{"IntegerLiteral", "RealLiteral"}).FindByShortName("0");

CxList binaryExprs = Find_BinaryExpr();
CxList indexer = Find_IndexerRefs();
CxList unknown = Find_Unknown_References();
CxList methods = Find_Methods();

CxList rand = methods.FindByShortName("rand");
CxList hasTarget = rand.GetMembersWithTargets();
CxList stdTarget = unknown.FindByShortName("std");
hasTarget -= hasTarget.GetMembersWithTargets(stdTarget);

rand -= hasTarget;
zero.Add(rand);

methods -= rand;

// div and ldiv functions from stdlib
String[] divs = new string[] {"div","ldiv"};
CxList divFunctions = Find_Members_By_Include("stdlib.h", divs);
CxList stdLibDivSecParam = All.GetParameters(divFunctions, 1);
CxList divParams = stdLibDivSecParam.FindByType(typeof(IntegerLiteral));
result.Add(divParams * zero);

// Methods without definition are considered sanitizers to reduce false positives
CxList metDefinitions = All.FindByType(typeof(MethodDecl)).FindDefinition(methods);
methods -= methods.FindAllReferences(metDefinitions);
methods -= Find_Members_By_Include("stdlib.h", new string[] {"atoi","atof","atol","atoll"});

CxList sanitize = All.NewCxList();
sanitize.Add(binaryExprs);
sanitize.Add(indexer);
sanitize.Add(methods);
sanitize.Add(Find_Null_Pointer_As_Zero());

// Only direct assign(a=0), multiply assign(a*=0) and logical and assignment(a&&=0) 
// set the value of a to 0 for sure. Other assignments are considered sanitizers.
CxList additionAssign = All.NewCxList();
CxList assignExpressions = Find_AssignExpr();
CxList divideAndModulusAssigns = All.NewCxList();

foreach(CxList assignExpression in assignExpressions){
	
	AssignExpr assignExpressionDOM = assignExpression.TryGetCSharpGraph<AssignExpr>();
	var op = assignExpressionDOM.Operator;
	
	if(op != AssignOperator.Assign && op != AssignOperator.MultiplyAssign){
		
		sanitize.Add(assignExpression);
		
		if(op == AssignOperator.DivisionAssign || op == AssignOperator.ModulusAssign || op == AssignOperator.AndAssign){
			// Divide assignments(a/=0),modulus assignments(a%=0) and logical and assignment(a&&=0) 
			// are vulnerable and will be used later on.
			divideAndModulusAssigns.Add(assignExpressionDOM.Right.DomId, assignExpressionDOM.Right);
		}
	}
}

// Dead code is sanitized
CxList deadBlocks = Find_Dead_Blocks_From_Conditions();
sanitize.Add(All.GetByAncs(deadBlocks));

// When abstract interpretation is able to calculate a value that does not contain 0, consider it sanitized.
IntegerIntervalAbstractValue zeroAbstractValue = new IntegerIntervalAbstractValue(0);
sanitize.Add(All.FindByAbstractValue(
	_ => (_ is IntegerIntervalAbstractValue && !_.Contains(zeroAbstractValue)) || _ is NoneAbstractValue));





CxList rightToDiv = All.NewCxList();
rightToDiv.Add(divideAndModulusAssigns);
foreach(CxList binaryExpr in binaryExprs)
{
	BinaryExpr binaryExprDOM = binaryExpr.TryGetCSharpGraph<BinaryExpr>();
	if (binaryExprDOM != null && binaryExprDOM.Operator == BinaryOperator.Divide || binaryExprDOM.Operator == BinaryOperator.Modulus)
	{		
		Expression right = binaryExprDOM.Right;
		if (right != null)
		{			
			rightToDiv.Add(right.NodeId, right);
		}
	}
}

// When abstract interpretation is able to calculate a value for the right side of assign expressions
// such as a/=0 and a%=0 as well as right side of divide and modulus operations, add it to the results.
CxList zeroAbsVal = rightToDiv.FindByAbstractValue(
	_ => _ is IntegerIntervalAbstractValue && _.Contains(zeroAbstractValue));
rightToDiv -= zeroAbsVal;

result.Add(zeroAbsVal);

//--------------------------
//Numeric variables whose abstract value is AnyAbstractValues may contain 0, so they should be part of the result
CxList possibleZeros = (Find_Integers() * unknown).FindByAbstractValue(_ => _ is AnyAbstractValue);
possibleZeros = possibleZeros.FindByTypes(new string[] {"float","double"});

CxList possibleZerosToRemove = All.NewCxList();

//Find possible zeros that are in the context of an IF/ELSE/Loop where it appears also in the condition 
CxList possibleZerosInConditions = Get_Conditions() * possibleZeros;
CxList possibleZerosInConditionalStmts = possibleZeros.GetByAncs(Find_Ifs() + Find_IterationStmt());
foreach(CxList possibleZero in possibleZerosInConditions){
	CxList condStmt = possibleZero.GetAncOfType(typeof(IfStmt));
	condStmt.Add(possibleZero.GetAncOfType(typeof(IterationStmt)));
	CxList san = possibleZerosInConditionalStmts.GetByAncs(condStmt) * unknown.FindAllReferences(possibleZero);
	possibleZerosToRemove.Add(san);
}

//Find all constants
possibleZerosToRemove.Add(unknown.FindAllReferences(Find_Declarators().GetByAncs(Find_Constants())));

//Find all possibleZeros that are initialized with a specific real value different from 0
CxList varsInitWithNoZero = All.FindDefinition(possibleZeros).FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList allRealLiterals = All.FindByType(typeof(RealLiteral)) - zero;
varsInitWithNoZero = allRealLiterals.GetAssignee(); 
possibleZerosToRemove.Add(possibleZeros.FindAllReferences(varsInitWithNoZero));

possibleZeros -= possibleZerosToRemove;
result.Add(rightToDiv * possibleZeros);
//---------------------

// Add real literal zeros (0.0) on the right side of divide operations to the results.
CxList literalZeros = rightToDiv * zero;
rightToDiv -= literalZeros;
result.Add(literalZeros);

// Add references that derive from 0 to the list of potential divide by zero vulnerabilities.
// Add references that derive from inputs to the list of potential divide by zero vulnerabilities.
CxList inputs = Find_Interactive_Inputs();

//Standard methods known to return numeric values without changing possible zero values
//which are on the right of a division
CxList stdMethods = Find_Members_By_Include("Math.hpp", new string[] {"abs","pow"});
stdMethods.Add(Find_Members_By_Include("stdlib.h", new string[] {"abs"}));
stdMethods = stdMethods * Find_Methods() * rightToDiv;
result.Add(stdMethods.FindByParameters(unknown.GetParameters(stdMethods, 0) * possibleZeros));

//remove the standard methods from sanitizers
sanitize -= stdMethods;

CxList pointerDiv = rightToDiv.FindByType(typeof(UnaryExpr)).FindByShortName("Pointer");

CxList divider = unknown * rightToDiv;
divider.Add(stdMethods);
divider.Add(unknown.FindByFathers(pointerDiv));
divider.Add(stdLibDivSecParam);
CxList dividerFlow = divider.InfluencedByAndNotSanitized(zero + inputs, sanitize);
result.Add(dividerFlow);