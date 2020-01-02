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
CxList unknown = All.FindByType(typeof(UnknownReference));
CxList equal = methods.FindByShortName("equals");

CxList allTrue = All.FindByShortName("true");
CxList allFalse = All.FindByShortName("false");
CxList objectCreate = (allTrue + allFalse).GetAncOfType(typeof(ObjectCreateExpr));

allTrue -= allTrue.GetByAncs(objectCreate);
allFalse -= allFalse.GetByAncs(objectCreate);

// Find true conditions
CxList True = conditions * allTrue;

CxList notAlwaysSetType = 
	All.FindByType(typeof(Catch)) +
	All.FindByType(typeof(IfStmt)) +
	All.FindByType(typeof(IterationStmt)) +
	All.FindByType(typeof(Case)) +
	Param +
	All.FindByType(typeof(ForEachStmt));
CxList notAlwaysSet = All.GetByAncs(notAlwaysSetType);

CxList allTrueNotAlwaysSet = allTrue.GetByAncs(notAlwaysSetType);
allTrueNotAlwaysSet -= allTrueNotAlwaysSet.GetByAncs(conditions);

CxList undeclaredMethods = methods - methods.FindDefinition(All.FindAllReferences(methods));
CxList parameters = All.GetByAncs(undeclaredMethods) - undeclaredMethods;

CxList sanitizer = binaryExpr + unaryExpr + equal + parameters;
CxList trueByInfluence = conditions.InfluencedByAndNotSanitized(allTrue - allTrueNotAlwaysSet, sanitizer);
CxList ifStmt = All.FindByType(typeof(IfStmt));
CxList assignInIf = assign.GetByAncs(ifStmt);
trueByInfluence -= trueByInfluence.DataInfluencedBy(unknown.GetByAncs(assignInIf).FindByAssignmentSide(CxList.AssignmentSide.Left));

True.Add(trueByInfluence);

True.Add(conditions.FindAllReferences(allTrue.GetAncOfType(typeof(ConstantDecl))));
CxList trueDef = allTrue.GetAncOfType(typeof(FieldDecl));
CxList trueReferences = conditions.FindAllReferences(trueDef);

// Find all true references that are influenced by something, thus might not always be false
CxList unknownMethods = methods - methods.FindAllReferences(All.FindDefinition(methods));
CxList unknownMethodsIs = unknownMethods.FindByShortName("is*", false);
CxList unknownMethodsNotIs = unknownMethods - unknownMethodsIs;
CxList influencing = 
	unknownMethodsIs +
	allFalse +
	Find_Plain_Interactive_Inputs() +
	Find_Plain_DB_Out() +
	Find_Plain_Read_DB() +
	Find_Plain_Read_NonDB();
CxList trueRefInfluenced = (trueReferences - trueDef).InfluencedByAndNotSanitized(influencing, unknownMethodsNotIs);
True.Add(trueReferences - trueRefInfluenced);

CxList setters = All.FindByType(typeof(MethodDecl)).FindByShortName("set*");
CxList inSetters = All.GetByAncs(setters);
CxList paramInSetters = inSetters.FindByType(typeof(ParamDecl));
CxList trueInSetters = inSetters.FindAllReferences(trueDef);
trueInSetters = trueInSetters.DataInfluencedBy(paramInSetters);
True -= True.FindAllReferences(trueInSetters);

True -= True.DataInfluencedBy(allFalse);

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
			if (binary.Operator == BinaryOperator.IdentityInequality)
			{
				CxList stringsHere = stringInMethods.GetByAncs(thisMethod);
				CxList affectedByString = unknownIsNull * unknownIsNull.InfluencedByAndNotSanitized(stringsHere, nullSanitizer);
				affectedByString -= affectedByString.DataInfluencedBy(notAlwaysSetHere);
				if (affectedByString.Count > 0)
				{
					True.Add(binary.NodeId, binary);
				}
			}
			else if (binary.Operator == BinaryOperator.IdentityEquality)
			{
				CxList nullHere = nullInMethods.GetByAncs(thisMethod);
				CxList affectedByNull = unknownIsNull * unknownIsNull.InfluencedByAndNotSanitized(nullHere, nullSanitizer);
				affectedByNull -= affectedByNull.DataInfluencedBy(notAlwaysSetHere);
				if (affectedByNull.Count > 0)
				{
					True.Add(binary.NodeId, binary);
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
				if (binary.Operator == BinaryOperator.IdentityEquality)
				{
					if (leftExp.Text == rightExp.Text)
					{
						True.Add(binary.NodeId, binary);
					}
				}
				else if (isNumber)
				{
					if (binary.Operator == BinaryOperator.IdentityInequality)
					{
						if (leftExp.Text != rightExp.Text)
						{
							True.Add(binary.NodeId, binary);
						}
					}
					if (binary.Operator == BinaryOperator.GreaterThanOrEqual)
					{
						if (leftNum >= rightNum)
						{
							True.Add(binary.NodeId, binary);
						}
					}
					else if (binary.Operator == BinaryOperator.GreaterThan)
					{
						if (leftNum > rightNum)
						{
							True.Add(binary.NodeId, binary);
						}
					}
					else if (binary.Operator == BinaryOperator.LessThanOrEqual)
					{
						if (leftNum <= rightNum)
						{
							True.Add(binary.NodeId, binary);
						}
					}
					else if (binary.Operator == BinaryOperator.LessThan)
					{
						if (leftNum < rightNum)
						{
							True.Add(binary.NodeId, binary);
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

True.Add(Find_Same_Switch_And_Case());

// Remove methods with return in an if statement
CxList trueMethod = All.FindDefinition(True.FindByType(typeof(MethodInvokeExpr)));
CxList ifWithReturn = All.FindByType(typeof(ReturnStmt)).GetByAncs(trueMethod).GetByAncs(ifStmt);
CxList wrongTrueMethod = ifWithReturn.GetAncOfType(typeof(MethodDecl));

result = True - True.FindAllReferences(wrongTrueMethod);