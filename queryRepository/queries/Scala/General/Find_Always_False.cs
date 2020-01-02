CxList assign = All.FindByType(typeof(AssignExpr));
CxList integers = All.FindByType(typeof(IntegerLiteral));
CxList conditions = Find_Conditions();
CxList constants = All.FindByType(typeof(ConstantDecl));
CxList inConstants = All.GetByAncs(constants);
CxList binaryExpr = All.FindByType(typeof(BinaryExpr));
CxList methods = Find_Methods();
CxList strings = Find_Strings();
CxList nul = All.FindByType(typeof(NullLiteral));
CxList Param = All.FindByType(typeof(Param));
CxList ParamDecl = All.FindByType(typeof(ParamDecl));


CxList unaryExpr = All.FindByType(typeof(UnaryExpr));
CxList unknown = Find_UnknownReference();
CxList equal = methods.FindByShortName("equals");

CxList allTrue = All.FindByShortName("true");
CxList allFalse = All.FindByShortName("false");
CxList objectCreate = (allTrue + allFalse).GetAncOfType(typeof(ObjectCreateExpr));

//Find Ternary (Conditional) Expressions
CxList ternary_exps = All.FindByType(typeof(TernaryExpr));
CxList expr_in_ter = All.NewCxList();

foreach (CxList tr in ternary_exps)
{
	TernaryExpr ter = tr.data.GetByIndex(0) as TernaryExpr;
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

// Find false conditions
CxList False = conditions * allFalse;

CxList notAlwaysSetType = 
	All.FindByType(typeof(Catch)) +
	All.FindByType(typeof(IfStmt)) +
	All.FindByType(typeof(IterationStmt)) +
	All.FindByType(typeof(Case)) +
	Param +
	All.FindByType(typeof(ForEachStmt));
CxList notAlwaysSet = All.GetByAncs(notAlwaysSetType);

CxList allFalseNotAlwaysSet = allFalse.GetByAncs(notAlwaysSetType);
allFalseNotAlwaysSet -= allFalseNotAlwaysSet.GetByAncs(conditions);

CxList undeclaredMethods = methods - methods.FindDefinition(All.FindAllReferences(methods));
CxList parameters = All.GetByAncs(undeclaredMethods) - undeclaredMethods;

CxList sanitizer = expr_in_ter + binaryExpr + unaryExpr + equal + parameters;
CxList falseByInfluence = conditions.InfluencedByAndNotSanitized(allFalse - allFalseNotAlwaysSet, sanitizer);
CxList ifStmt = All.FindByType(typeof(IfStmt));
CxList assignInIf = assign.GetByAncs(ifStmt);
falseByInfluence -= falseByInfluence.DataInfluencedBy(unknown.GetByAncs(assignInIf).FindByAssignmentSide(CxList.AssignmentSide.Left));

False.Add(falseByInfluence);

False.Add(conditions.FindAllReferences(allFalse.GetAncOfType(typeof(ConstantDecl))));
CxList falseDef = allFalse.GetAncOfType(typeof(FieldDecl));
CxList falseReferences = conditions.FindAllReferences(falseDef);

// Find all false references that are influenced by something, thus might not always be false
CxList unknownMethods = methods - methods.FindAllReferences(All.FindDefinition(methods));
CxList unknownMethodsIs = unknownMethods.FindByShortName("is*", false);
CxList unknownMethodsNotIs = unknownMethods - unknownMethodsIs;
CxList influencing = 
	unknownMethodsIs +
	allTrue +
	Find_Plain_Interactive_Inputs() +
	Find_Plain_DB_Out() +
	Find_Plain_Read_DB() +
	Find_Plain_Read_NonDB();


CxList falseRefInfluenced = (falseReferences - falseDef).InfluencedByAndNotSanitized(influencing, unknownMethodsNotIs);
False.Add(falseReferences - falseRefInfluenced);

CxList setters = Find_MethodDeclaration().FindByShortName("set*");
CxList inSetters = All.GetByAncs(setters);
CxList paramInSetters = inSetters.FindByType(typeof(ParamDecl));
CxList falseInSetters = inSetters.FindAllReferences(falseDef);
falseInSetters = falseInSetters.DataInfluencedBy(paramInSetters);
False -= False.FindAllReferences(falseInSetters);
	

False -= False.DataInfluencedBy(allTrue);

CxList intDecl =
	integers.GetAncOfType(typeof(FieldDecl)) + 
	integers.GetAncOfType(typeof(ConstantDecl)) +
	integers.GetAncOfType(typeof(VariableDecl));

intDecl = 
	intDecl.FindByFieldAttributes(Modifiers.Private) +
	intDecl.FindByFieldAttributes(Modifiers.Protected);

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
CxList unknownHere = unknown.FindByFathers(bin);
CxList nullSanitizer = binaryExpr;
nullSanitizer -= nullSanitizer.GetByAncs(conditions);
nullSanitizer = nullSanitizer.GetByAncs(binMethods);
CxList notAlwaysSetInMethod = notAlwaysSet.GetByAncs(binMethods).GetByAncs(assign).FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList paramOfMethods = ParamDecl.GetByAncs(binMethods);
nullSanitizer.Add(paramOfMethods);
nullSanitizer -= unknownHere;
CxList constantInAllConditions = inBin.FindAllReferences(constants + constantIntInCondition);
CxList constantInLeftSideOfAssignment = All.FindAllReferences(inConstants).FindByAssignmentSide(CxList.AssignmentSide.Left) - inConstants;
constantInAllConditions -= constantInLeftSideOfAssignment;


foreach (CxList b in bin)
{
	try
	{
		BinaryExpr binary = b.data.GetByIndex(0) as BinaryExpr;
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
			//CxList paramHere = paramOfMethods.GetByAncs(thisMethod);
			//CxList sanitizerHere = nullSanitizer.GetByAncs(thisMethod) + paramHere - unknownIsNull;
			//CxList sanitizerHere = nullSanitizer + paramOfMethods - unknownIsNull;
			//CxList sanitizerHere = nullSanitizer;// -unknownIsNull;
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
			bool rightIsNum = int.TryParse(rightExp.Text, out leftNum);
		
			bool isNumber = rightIsNum && leftIsNum;
			bool isUnknown = left is UnknownReference && right is UnknownReference;

			CxList constantInCondition = constantInAllConditions.GetByAncs(b);
			if (constantInCondition.Count > 0 && (leftIsNum || rightIsNum))
			{
				constantInCondition = inConstants.GetByAncs(inConstants.FindDefinition(left + right)).FindByType(typeof(IntegerLiteral));
				//constantInCondition = constantInCondition.GetByAncs(constants);

				if (constantInCondition.Count > 0)
				{
					CSharpGraph il = constantInCondition.data.GetByIndex(0) as IntegerLiteral;
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
CxList ifWithReturn = All.FindByType(typeof(ReturnStmt)).GetByAncs(falseMethod).GetByAncs(ifStmt);
CxList wrongFalseMethod = ifWithReturn.GetAncOfType(typeof(MethodDecl));

result = False - False.FindAllReferences(wrongFalseMethod);