/* MISRA CPP RULE 6-5-2
 ------------------------------
 This query returns all for loops that their loop counter is not modified by -- or ++ but have loop counters
 that within condition are not used by the operands >,>= ,<,<=

 The Example below shows code with vulnerability: 

      
    for (int i=0; i!=10; i+=2);   //non-compliant

    for(int k=0; k!=110|| k<f+3| k!=90+1; f=1,k++,k+=10) //non-compliant
*/

//finds all for statements
CxList allFors = All.FindByType(typeof(IterationStmt));
CxList helper = allFors;
foreach(CxList allf in allFors)
{

	IterationStmt i = allf.data.GetByIndex(0) as IterationStmt;
	if(i != null)
	{
		IterationType it = i.IterationType;
		if(!it.ToString().Equals("For"))
		{
			helper -= allf;
		}
	}
	
}
allFors = helper;

CxList totalUr = All.FindByType(typeof(UnknownReference));
CxList unrf = totalUr.GetByAncs(allFors);
CxList totalUn = All.FindByType(typeof(UnaryExpr));
CxList unaryEx = totalUn.GetByAncs(allFors);
CxList totalPf = All.FindByType(typeof(PostfixExpr));
CxList totalEx = totalPf.GetByAncs(allFors);
CxList totalExpr = All.FindByType(typeof(ExprStmt));
CxList expr = totalExpr.GetByAncs(allFors);
CxList assignEx = All.FindByType(typeof(AssignExpr)).GetByAncs(allFors);
CxList methInvEx = All.FindByType(typeof(MethodInvokeExpr)).GetByAncs(allFors);
CxList bo = All.GetByBinaryOperator(BinaryOperator.IdentityEquality) +
	All.GetByBinaryOperator(BinaryOperator.IdentityInequality);
CxList leftSide = All.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList declarators = Find_All_Declarators().GetByAncs(allFors);

//finds all increment statements inside the for
foreach(CxList cur in allFors)
{
	CxList increment = All.NewCxList();
	IterationStmt iterS = cur.data.GetByIndex(0) as IterationStmt;
	StatementCollection incrementColl = iterS.Increment;
	for(int i = 0; incrementColl != null && i < incrementColl.Count; i++)
	{	
		if(incrementColl[i] != null)
		{
			increment.Add(All.FindById(incrementColl[i].NodeId));
		}
	}
	CxList illegalExpr = assignEx.GetByAncs(increment) + methInvEx.GetByAncs(increment);
	CxList refInIncr = unrf.GetByAncs(illegalExpr);
	CxList left = refInIncr.FindByAssignmentSide(CxList.AssignmentSide.Left);
	CxList prm = refInIncr.GetParameters(methInvEx);
	CxList toCheck = prm + left;
	
	//get's for this "for" statement its "test" statement
	Expression exp = iterS.Test;
	CxList test = All.NewCxList();
	if(exp != null){
		test.Add(All.FindById(exp.NodeId));
	}
	//checks the operators
	CxList binary = test.FindByType(typeof(BinaryExpr));
	CxList legal = binary.GetByBinaryOperator(BinaryOperator.LessThan) +
		binary.GetByBinaryOperator(BinaryOperator.GreaterThan) +
		binary.GetByBinaryOperator(BinaryOperator.LessThanOrEqual) +
		binary.GetByBinaryOperator(BinaryOperator.GreaterThanOrEqual);
	CxList foundIllegal = binary - legal;
	//handles the case that there is or or and in the test
	CxList special = foundIllegal.GetByBinaryOperator(BinaryOperator.BooleanOr) +
		foundIllegal.GetByBinaryOperator(BinaryOperator.BooleanAnd) +
		foundIllegal.GetByBinaryOperator(BinaryOperator.BitwiseOr) +
		foundIllegal.GetByBinaryOperator(BinaryOperator.BitwiseAnd);
	CxList illegal = foundIllegal - special;
	//retrieves the operators from the complex expression 
	CxList specialAnc = bo.GetByAncs(special);
	//adds them to the bunch
	illegal.Add(specialAnc);
	
	//gets the tests indexes and compares the two
	CxList testRef = unrf.GetByAncs(illegal);
	testRef.Add(methInvEx.GetByAncs(test).GetTargetOfMembers());
	testRef.Add(unrf.GetByAncs(methInvEx.GetByAncs(test)));
	CxList illegRef = toCheck;
	CxList isLC = All.NewCxList();
	
	isLC = testRef.FindAllReferences(illegRef);

	CxList init = All.NewCxList();
	StatementCollection initColl = iterS.Init;
	for(int i = 0; initColl != null && i < initColl.Count; i++)
	{
		if(initColl[i] != null)
		{
			init.Add(All.FindById(initColl[i].NodeId));
		}
	}
	
	//retrieves the loop counters
	CxList loopCounter = unrf.FindByFathers(init.FindByType(typeof(ExprStmt)));

	CxList unknownRef = unrf.GetByAncs(init);

	CxList leftAsn = unknownRef * leftSide;

	loopCounter.Add(leftAsn + declarators.GetByAncs(init));
	result.Add(isLC);
}