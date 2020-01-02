/* MISRA CPP RULE 6-5-5
 ------------------------------
 This query finds if there are loop control varibales that are not loop counters that are modified within condition
 or expression.

 The following example shows what main code should look like: 
     

       for(int j=0; n-100; j++);       //non-compliant 

       for(int j=0;  bar(&n); j+=10);  //non-compliant
 
       for(int j=0;  j<100; n+=10);    //non-compliant



*/

//gets all for statements

CxList allFors = All.FindByType(typeof(IterationStmt));
CxList helper = allFors;
foreach(CxList allf in allFors)
{
	IterationStmt i = allf.data.GetByIndex(0) as IterationStmt;
	if(i != null)
	{
		IterationType it = i.IterationType;
		if(!it.ToString().Equals("For"))
			helper -= allf;
	}
}
allFors = helper;

	CxList rf = All.FindByType(typeof(UnknownReference)) + Find_All_Declarators();
	CxList unrf = rf.GetByAncs(allFors);
	CxList totalAsns = All.FindByType(typeof(AssignExpr));
	CxList asns = totalAsns.GetByAncs(allFors);
	CxList totalUn = All.FindByType(typeof(UnaryExpr));
	CxList unaryEx = totalUn.GetByAncs(allFors);
	CxList totalPf = All.FindByType(typeof(PostfixExpr));
	CxList postfixEx = totalPf.GetByAncs(allFors);
	CxList declarators = Find_All_Declarators().GetByAncs(allFors);
	CxList leftSd = All.FindByAssignmentSide(CxList.AssignmentSide.Left);

foreach(CxList cur in allFors)
{
	//finds the test element
	IterationStmt iterA = cur.data.GetByIndex(0) as IterationStmt;
	CxList testExpr = All.NewCxList();
	CxList incrExpr = All.NewCxList();
	CxList init = All.NewCxList();
	
	if(iterA != null)
	{
		Expression expr = iterA.Test;
		if(expr != null)
		{
			testExpr.Add(All.FindById(expr.NodeId));
		}	
		//finds the increment element
		StatementCollection incrementColl = iterA.Increment;
		for(int i = 0; incrementColl != null && i < incrementColl.Count; i++)
		{
			if(incrementColl[i] != null)
			{
				incrExpr.Add(All.FindById(incrementColl[i].NodeId));
			}
		}
		
		StatementCollection initColl = iterA.Init;
		for(int i = 0; initColl != null && i < initColl.Count; i++)
		{
			if(initColl[i] != null)
			{
				init.Add(All.FindById(initColl[i].NodeId));
			}
		}
	}
	
	CxList loopCounter = unrf.FindByFathers(init.FindByType(typeof(ExprStmt)));

	CxList unknownRef = unrf.GetByAncs(init);
	CxList leftAsn = unknownRef * leftSd;
	loopCounter.Add(leftAsn + declarators.GetByAncs(init));
	CxList lcv = loopCounter;
	
	//all references in statement
	
	CxList incrUn = unrf.GetByAncs(incrExpr);
	CxList testUn = unrf.GetByAncs(testExpr);
	CxList testRef = incrUn + testUn;
	
	CxList tmp = lcv.FindAllReferences(testRef) - testRef;
	
	testRef -= testRef.FindAllReferences(tmp) - tmp;
	
	incrUn = testRef.GetByAncs(incrExpr);
	testUn = testRef.GetByAncs(testExpr);
	
	CxList rmv = testUn.FindAllReferences(incrUn);
	CxList rmv2 = incrUn.FindAllReferences(testUn);
	testRef -= (rmv + rmv2);
	if(testRef.Count == 0)
	{
		continue;
	}
	
	//finds bar(&n) in the test element
	CxList test = testRef.GetFathers().FindByType(typeof(UnaryExpr)).FindByShortName("Address");
	test = test.GetAncOfType(typeof(MethodInvokeExpr));
	CxList reference = testRef.GetByAncs(test);

	//for+= =
	CxList testLeftAsn = All.NewCxList();
	CxList assn = asns.GetByAncs(testExpr)
		+ asns.GetByAncs(incrExpr);
	
	testLeftAsn.Add(testRef.FindByFathers(assn) * leftSd);
	reference.Add(testLeftAsn);
	//for ++n,--n
	CxList unary = unaryEx.GetByAncs(testExpr) +
		unaryEx.GetByAncs(incrExpr);
	unary = unary.FindByShortName("Decrement") +
		unary.FindByShortName("Increment");
	reference.Add(testRef.GetByAncs(unary).FindByType(typeof(UnknownReference)));
	//for n++,n--
	CxList pE = postfixEx.GetByAncs(testExpr)
		+ postfixEx.GetByAncs(incrExpr);
	CxList postRef = All.NewCxList();
	foreach(CxList w in pE)
	{
		PostfixExpr pf = w.data.GetByIndex(0)as PostfixExpr;
		if(pf != null)
		{
			Expression left = pf.Left;
			if(left != null)
			{
				postRef.Add(All.FindById(left.NodeId));
			}
		}
	}
	reference.Add(postRef * testRef);
	
	//checks if the found illegal loop control variable  is not the loop counter 
	//and in case it's not adds it to the result.
	result.Add(reference - reference.FindAllReferences(lcv));

}