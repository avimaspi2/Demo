/* MISRA CPP RULE 6-5-6
 ------------------------------
 This query finds if there are loop control varibales that are not loop counters that are modified inside the for statement
 and are not of type bool

 The following example shows what main code should look like: 
     


    class A{
      public:
           int a;
           void rrr()
           {
               for(int x=0; x<100 && a==10;x++)    //non-compliant
                {
                     a++;
                }
           }
    };



      for (int i=0; i<100; i-=n)  //non-compliant
      {  
           n+=10;
           bar(&n);
      }

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

CxList totalUnknownR = All.FindByType(typeof(UnknownReference)) + Find_All_Declarators() + All.FindByType(typeof(FieldDecl));
CxList unrf = totalUnknownR.GetByAncs(allFors);
CxList unrfWithMA = unrf + All.FindByType(typeof(MemberAccess)).GetByAncs(allFors);
CxList declarators = Find_All_Declarators().GetByAncs(allFors);
CxList postFix = All.FindByType(typeof(PostfixExpr)).GetByAncs(allFors);
CxList typeR = All.FindByType(typeof(TypeRef));
CxList leftSd = All.FindByAssignmentSide(CxList.AssignmentSide.Left);

//first we find all loop control variables (appear in condition and iteration) that are modified inside statement
foreach(CxList cur in allFors)
{	
	CxList eis = unrfWithMA.GetByAncs(cur);
	CxList results = All.NewCxList();
	CxList testExpr = All.NewCxList();
	CxList incrExpr = All.NewCxList();
	CxList init = All.NewCxList();
	//finds the test element
	IterationStmt iterA = cur.data.GetByIndex(0) as IterationStmt;
	if(iterA != null)
	{
		Expression expr = iterA.Test;
		if(expr != null)
		{
			testExpr.Add(All.FindById(expr.NodeId));
		}
	
		//finds the increment element
		
		StatementCollection incrementColl = iterA.Increment;
		for(int i = 0;incrementColl != null && i < incrementColl.Count; i++)
		{
			if(incrementColl[i] != null)
			{
				incrExpr.Add(All.FindById(incrementColl[i].NodeId));
			}
		}
		//finds all init elements
		
		StatementCollection initColl = iterA.Init;
		for(int i = 0;initColl != null && i < initColl.Count; i++)
		{
			if(initColl[i] != null)
			{
				init.Add(All.FindById(initColl[i].NodeId));
			}
		}
	
	}
	
	CxList loopCounter = eis.FindByFathers(init.FindByType(typeof(ExprStmt)));

	CxList unknownRef = eis.GetByAncs(init);

	CxList leftAsn = unknownRef * leftSd;

	loopCounter.Add(leftAsn + declarators.GetByAncs(init));
	CxList lcv = loopCounter;

	//finds all references inside the statement
	CxList incrUn = eis.GetByAncs(incrExpr);
	CxList testUn = eis.GetByAncs(testExpr);
	CxList allRefsInFor = incrUn + testUn;
	
	CxList tmp = lcv.FindAllReferences(allRefsInFor) - allRefsInFor;
	allRefsInFor -= allRefsInFor.FindAllReferences(tmp) - tmp;
	
	incrUn = allRefsInFor.GetByAncs(incrExpr);
	testUn = allRefsInFor.GetByAncs(testExpr);
	
	CxList rmv = testUn.FindAllReferences(incrUn);
	CxList rmv2 = incrUn.FindAllReferences(testUn);
	allRefsInFor -= (rmv + rmv2);
	if(allRefsInFor.Count == 0)
	{
		continue;
	}
	
	allRefsInFor = eis.FindAllReferences(allRefsInFor) -
		(eis.GetByAncs(init) + eis.GetByAncs(incrExpr) + eis.GetByAncs(testExpr));
	//checks if they are modified inside the statement 
	CxList backToAsn = All.NewCxList();

	allRefsInFor -= allRefsInFor.GetMembersOfTarget().GetTargetOfMembers();
	CxList operation = allRefsInFor.GetFathers();
	
	CxList change = postFix.FindByFathers(allRefsInFor.GetAncOfType(typeof(ExprStmt)));
	
	change = allRefsInFor.GetByAncs(change.GetAncOfType(typeof(ExprStmt)));
	change.Add(operation.FindByType(typeof(UnaryExpr)).FindByName("Increment") +
		operation.FindByType(typeof(UnaryExpr)).FindByName("Decrement"));
	backToAsn.Add(allRefsInFor.FindByAssignmentSide(CxList.AssignmentSide.Left));
	change.Add(operation.FindByType(typeof(UnaryExpr)).FindByName("Address").GetAncOfType(typeof(MethodInvokeExpr)));
	backToAsn.Add(allRefsInFor.GetByAncs(change.GetAncOfType(typeof(ExprStmt))));
	
	//checks if the found illegal loop control variable  is not the loop counter 
	//and in case it's not adds it to the result.
	
	results = backToAsn;
	CxList temp = totalUnknownR.FindDefinition(results);
	foreach(CxList t in temp)
	{
		CSharpGraph g = t.data.GetByIndex(0) as CSharpGraph;
		if(g != null)
		{
			if(!g.TypeName.Equals("bool"))
			{
				result.Add(cur);
			}
		}
	}
}