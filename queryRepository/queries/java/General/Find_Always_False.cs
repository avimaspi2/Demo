CxList assign = Find_AssignExpr();
CxList integers = Find_IntegerLiterals();
CxList catchStmts = base.Find_Catch();
CxList varDeclsStmts = base.Find_VariableDeclStmt();
CxList conditions = Find_Conditions();
CxList constants = Find_Constants();
CxList inConstants = All.GetByAncs(constants);
CxList binaryExpr = base.Find_BinaryExpr();
CxList methods = Find_Methods();
CxList strings = Find_Strings();
CxList nul = base.Find_NullLiteral();
CxList unaryExpr = base.Find_Unarys();
CxList unknown = Find_UnknownReference();
CxList Param = Find_Params();
CxList ParamDecl = Find_ParamDeclaration();
CxList ifStmt = base.Find_Ifs();
CxList decls = Find_MethodDeclaration();

CxList varDeclStmtsInCatch = varDeclsStmts.FindByFathers(catchStmts);
nul -= nul.GetByAncs(varDeclStmtsInCatch);

CxList equal = methods.FindByShortName("equals");

CxList allTrue = All.FindByShortName("true");
CxList allFalse = All.FindByShortName("false");

CxList allTruesFalses = All.NewCxList();
allTruesFalses.Add(allTrue, allFalse);

CxList objectCreate = allTruesFalses.GetAncOfType(typeof(ObjectCreateExpr));

//Find Ternary (Conditional) Expressions
CxList ternary_exps = base.Find_TernaryExpr();
CxList expr_in_ter = All.NewCxList();

foreach (CxList tr in ternary_exps)
{
	TernaryExpr ter = tr.TryGetCSharpGraph<TernaryExpr>();
	Expression ter_true = ter.True;
	Expression ter_false = ter.False;
	
	expr_in_ter.Add(ter_true.NodeId, ter_true);
	expr_in_ter.Add(ter_false.NodeId, ter_false);
}


expr_in_ter = All.GetByAncs(expr_in_ter);

allTrue -= allTrue.GetByAncs(objectCreate);
allFalse -= allFalse.GetByAncs(objectCreate);

//Remove expressions from ternary conditionals
allTrue -= expr_in_ter;
allFalse -= expr_in_ter;

//Remove the Boolean Literals where the uknown reference could be reassigned 	
CxList decl = allFalse.GetAncOfType(typeof(Declarator));
CxList refs = unknown.FindAllReferences(decl);
CxList lefts = refs.FindByAssignmentSide(CxList.AssignmentSide.Left);
lefts = decl.FindDefinition(lefts);
CxList bool_in_declarator = allFalse.GetByAncs(lefts);

allFalse -= bool_in_declarator;

// Find false conditions
CxList False = conditions * allFalse;

CxList notAlwaysSetType = All.NewCxList();
notAlwaysSetType.Add(Param, catchStmts, ifStmt);
notAlwaysSetType.Add(base.Find_IterationStmt());
notAlwaysSetType.Add(base.Find_Cases());
notAlwaysSetType.Add(base.Find_ForEachStmt());

CxList notAlwaysSet = All.GetByAncs(notAlwaysSetType);

CxList allFalseNotAlwaysSet = allFalse.GetByAncs(notAlwaysSetType);
allFalseNotAlwaysSet -= allFalseNotAlwaysSet.GetByAncs(conditions);

CxList undeclaredMethods = methods - methods.FindDefinition(All.FindAllReferences(methods));
CxList parameters = All.GetByAncs(undeclaredMethods) - undeclaredMethods;

CxList sanitizer = All.NewCxList();
sanitizer.Add(expr_in_ter, binaryExpr, unaryExpr, equal, parameters);

CxList falseByInfluence = conditions.InfluencedByAndNotSanitized(allFalse - allFalseNotAlwaysSet, sanitizer);
CxList assignInIf = assign.GetByAncs(ifStmt);
falseByInfluence -= falseByInfluence.DataInfluencedBy(unknown.GetByAncs(assignInIf).FindByAssignmentSide(CxList.AssignmentSide.Left));

False.Add(falseByInfluence);

False.Add(conditions.FindAllReferences(allFalse.GetAncOfType(typeof(ConstantDecl)) + 
	allFalse.GetAncOfType(typeof(FieldDecl)) * constants));

CxList falseDef = allFalse.GetAncOfType(typeof(FieldDecl)) * Find_Field_Decl();
CxList falseReferences = conditions.FindAllReferences(falseDef);

// Find all false references that are influenced by something, thus might not always be false
CxList unknownMethods = methods - methods.FindAllReferences(All.FindDefinition(methods));
CxList unknownMethodsIs = unknownMethods.FindByShortName("is*", false);
CxList unknownMethodsNotIs = unknownMethods - unknownMethodsIs;


CxList influencing = Find_Plain_Interactive_Inputs();
influencing.Add(unknownMethodsIs, allTrue);
influencing.Add(Find_Plain_DB_Out());
influencing.Add(Find_Plain_Read_DB());
influencing.Add(Find_Plain_Read_NonDB());


CxList falseRefInfluenced = (falseReferences - falseDef).InfluencedByAndNotSanitized(influencing, unknownMethodsNotIs);
False.Add(falseReferences - falseRefInfluenced);

CxList setters = decls.FindByShortName("set*");
CxList inSetters = All.GetByAncs(setters);
CxList paramInSetters = inSetters.FindByType(typeof(ParamDecl));
CxList falseInSetters = inSetters.FindAllReferences(falseDef);
falseInSetters = falseInSetters.DataInfluencedBy(paramInSetters);
False -= False.FindAllReferences(falseInSetters);
	

False -= False.DataInfluencedBy(allTrue);

CxList intDeclAll = integers.GetAncOfType(typeof(FieldDecl));
intDeclAll.Add(integers.GetAncOfType(typeof(ConstantDecl)));
intDeclAll.Add(integers.GetAncOfType(typeof(VariableDecl)));

CxList intDecl = intDeclAll.FindByFieldAttributes(Modifiers.Private);
intDecl.Add(intDeclAll.FindByFieldAttributes(Modifiers.Protected));

intDecl -= integers.FindAllReferences(All.GetByAncs(assign).FindAllReferences(intDecl));

CxList constantIntInCondition = All.FindAllReferences(intDecl).GetByAncs(conditions);
inConstants.Add(All.GetByAncs(intDecl));

CxList bin = conditions.FindByType(typeof(BinaryExpr));
CxList AllTry = All.GetAncOfType(typeof(TryCatchFinallyStmt));
CxList AllFinally = All.GetFinallyClause(AllTry);
bin -= bin.GetByAncs(AllFinally);
CxList inBin = All.GetByAncs(bin);
CxList binMethods = All.GetMethod(bin);
CxList stringInMethods = strings.GetByAncs(binMethods);
stringInMethods -= strings.GetByAncs(Param);
CxList nullInMethods = nul.GetByAncs(binMethods);
nullInMethods -= All.GetByAncs(Param);
nullInMethods -= nullInMethods.GetByAncs(ternary_exps);
CxList unknownHere = unknown.FindByFathers(bin);

CxList nullSanitizer = All.NewCxList();
nullSanitizer.Add(binaryExpr);

nullSanitizer -= nullSanitizer.GetByAncs(conditions);
nullSanitizer = nullSanitizer.GetByAncs(binMethods);
CxList notAlwaysSetInMethod = notAlwaysSet.GetByAncs(binMethods).GetByAncs(assign).FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList paramOfMethods = ParamDecl.GetByAncs(binMethods);
nullSanitizer.Add(paramOfMethods);
nullSanitizer -= unknownHere;

CxList allConstants = All.NewCxList();
allConstants.Add(constants, constantIntInCondition);

CxList constantInAllConditions = inBin.FindAllReferences(allConstants);
CxList constantInLeftSideOfAssignment = All.FindAllReferences(inConstants).FindByAssignmentSide(CxList.AssignmentSide.Left) - inConstants;
constantInAllConditions -= constantInLeftSideOfAssignment;

foreach (CxList b in bin)
{
	try
	{
		BinaryExpr binary = b.TryGetCSharpGraph<BinaryExpr>();
		CSharpGraph rightExp = binary.Right;
		CSharpGraph leftExp = binary.Left;
		if (rightExp == null || leftExp == null)
		{
			continue;
		}
		if (rightExp is NullLiteral)
		{
			CxList thisMethod = binMethods.GetMethod(b);
			CxList unknownIsNull = unknownHere.FindByFathers(b);
			CxList notAlwaysSetHere = notAlwaysSetInMethod.GetByAncs(thisMethod);
			if (binary.Operator == BinaryOperator.IdentityEquality)
			{
				CxList stringsHere = stringInMethods.GetByAncs(thisMethod);
				CxList affectedByString = unknownIsNull * unknownIsNull.InfluencedByAndNotSanitized(stringsHere, nullSanitizer);
				affectedByString -= affectedByString.DataInfluencedBy(notAlwaysSetHere);
				if (affectedByString.Count > 0)
				{
					False.Add(binary.NodeId, binary);
				}
			}
			else if (binary.Operator == BinaryOperator.IdentityInequality)
			{
				CxList nullHere = nullInMethods.GetByAncs(thisMethod);		
				CxList affectedByNull = unknownIsNull * unknownIsNull.InfluencedByAndNotSanitized(nullHere, nullSanitizer);
				affectedByNull -= affectedByNull.DataInfluencedBy(notAlwaysSetHere);
				if (affectedByNull.Count > 0)
				{
					False.Add(binary.NodeId, binary);
				}
			}
		}
		else
		{
			CxList right = All.FindById(rightExp.NodeId);
			CxList left = All.FindById(leftExp.NodeId);
			int leftNum = -1;
			bool leftIsNum = int.TryParse(leftExp.Text, out leftNum);
			int rightNum = -1;
			bool rightIsNum = int.TryParse(rightExp.Text, out rightNum);
		
			bool isNumber = rightIsNum && leftIsNum;
			bool isUnknown = left is UnknownReference && right is UnknownReference;

			CxList constantInCondition = constantInAllConditions.GetByAncs(b);
			if (constantInCondition.Count > 0 && (leftIsNum || rightIsNum))
			{
				CxList leftRight = All.NewCxList();
				leftRight.Add(left);
				leftRight.Add(right);
				
				constantInCondition = inConstants.GetByAncs(inConstants.FindDefinition(leftRight)).FindByType(typeof(IntegerLiteral));				

				if (constantInCondition.Count > 0)
				{
					CSharpGraph il = constantInCondition.TryGetCSharpGraph<IntegerLiteral>();
					if (leftIsNum)
					{
						rightExp = il;
					}
					if (rightIsNum)
					{
						leftExp = il;
					}
				}
			}

			if (isUnknown || isNumber || constantInCondition.Count > 0)
			{
				if (binary.Operator == BinaryOperator.IdentityInequality)
				{
					if (leftExp.Text == rightExp.Text)
					{
						False.Add(binary.NodeId, binary);
					}
				}
				else if (isNumber)
				{
					if (binary.Operator == BinaryOperator.IdentityEquality)
					{
						if (leftExp.Text != rightExp.Text)
						{
							False.Add(binary.NodeId, binary);
						}
					}
					if (binary.Operator == BinaryOperator.GreaterThanOrEqual)
					{
						if (leftNum < rightNum)
						{
							False.Add(binary.NodeId, binary);
						}
					}
					else if (binary.Operator == BinaryOperator.GreaterThan)
					{
						if (leftNum <= rightNum)
						{
							False.Add(binary.NodeId, binary);
						}	
					}
					else if (binary.Operator == BinaryOperator.LessThanOrEqual)
					{
						if (leftNum > rightNum)
						{
							False.Add(binary.NodeId, binary);
						}	
					}
					else if (binary.Operator == BinaryOperator.LessThan)
					{
						if (leftNum >= rightNum)
						{
							False.Add(binary.NodeId, binary);
						}	
					}
				}
			}
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
			
}

// Remove methods with return in an if statement
CxList falseMethod = All.FindDefinition(False.FindByType(typeof(MethodInvokeExpr)));
CxList ifWithReturn = Find_ReturnStmt().GetByAncs(falseMethod).GetByAncs(ifStmt);
CxList wrongFalseMethod = ifWithReturn.GetAncOfType(typeof(MethodDecl));

result = False - False.FindAllReferences(wrongFalseMethod);

//We remove flows that go through method invokes which are not mapped to method decl && a flow exists between 
//the previous node to the method invoke.

CxList invokes = All.NewCxList();
invokes.Add(methods);
invokes -= invokes.FindAllReferences(decls);
invokes = invokes.GetMembersWithTargets();

CxList invokeTargets = invokes.GetTargetOfMembers();

CxList missingInvokeToDeclMapping = All.NewCxList();
foreach(CxList res in result.GetCxListByPath())
{
	CxList nodes = res.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes);
	CxList nodeInvoke = nodes * invokes;
	if (nodeInvoke.Count > 0)
	{
		CxList nodeTargets = nodes * invokeTargets;
		if (nodeInvoke.GetMembersWithTargets(nodeTargets).Count > 0)
		{
			missingInvokeToDeclMapping.Add(res);
		}
	}
}

result += All.FindByAbstractValue(abstractValue => abstractValue is FalseAbstractValue);
result -= missingInvokeToDeclMapping;