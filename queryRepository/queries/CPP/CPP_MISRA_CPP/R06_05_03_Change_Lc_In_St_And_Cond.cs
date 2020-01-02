/* MISRA CPP RULE 6-5-3
 ------------------------------
 This query results in all loop counters that are modified within condition or statement

 The Example below shows code with vulnerability: 

  for( j=0; j<100 && bar(&j); j+=foo()) //non-compliant
  {
        j++;      //non-compliant
        --j;      //non-compliant
        bar(&j);  //non-compliant
        j+=90;    //non-compliant
        j=j*7;    //non-compliant
        
    }
*/

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

	CxList postfix = All.FindByType(typeof(PostfixExpr)).GetByAncs(allFors);
	CxList unrf = All.FindByType(typeof(UnknownReference)).GetByAncs(allFors);
	CxList declarators = Find_All_Declarators().GetByAncs(allFors);
	CxList leftSide=All.FindByAssignmentSide(CxList.AssignmentSide.Left);

foreach(CxList cur in allFors)
{
		
	CxList init = All.NewCxList();
	IterationStmt iterA = cur.data.GetByIndex(0) as IterationStmt;
	StatementCollection initColl = iterA.Init;
		
	for(int i = 0; initColl != null && i < initColl.Count; i++)
	{
		if(initColl[i] != null)
		{
			init.Add(All.FindById(initColl[i].NodeId));
		}
	}

	CxList loopCounter = unrf.FindByFathers(init.FindByType(typeof(ExprStmt)));
	
	CxList unknownRef = unrf.GetByAncs(init);
	CxList leftAsn = unknownRef * leftSide;
	loopCounter.Add(leftAsn + declarators.GetByAncs(init));
	
	CxList allReferencesToLC = All.NewCxList();

	//retrieve the test part of the for loop
	CxList test = All.NewCxList();

	Expression testExp = iterA.Test;
	if(testExp != null)
	{
		test.Add(All.FindById(testExp.NodeId));
	}

	//retrieve the increment part of the for loop
	CxList increment = All.NewCxList();
	StatementCollection incrementColl = iterA.Increment;
	for(int i = 0; incrementColl != null && i < incrementColl.Count; i++)
	{	
		if(incrementColl[i] != null){
			increment.Add(All.FindById(incrementColl[i].NodeId));
		}
	}
	
	CxList testUn = unrf.GetByAncs(test);
	CxList incrUn = unrf.GetByAncs(increment);
	CxList additionalLC = incrUn.FindAllReferences(testUn) + testUn.FindAllReferences(incrUn);
	loopCounter.Add(additionalLC * testUn);
	
	CxList curExpr = unrf.GetByAncs(loopCounter.GetAncOfType(typeof(IterationStmt)));
	allReferencesToLC.Add(curExpr.FindAllReferences(loopCounter));
	CxList lCinCondition = allReferencesToLC.GetByAncs(test);
	
	//bar(&j)
	CxList asMethodParam = lCinCondition.GetByAncs(test).GetFathers().FindByName("Address");
	CxList theMethod = asMethodParam.GetAncOfType(typeof(MethodInvokeExpr));
	result.Add(theMethod);
	CxList allLCRefInsideStmt = allReferencesToLC 
		- allReferencesToLC.GetByAncs(init) 
		- allReferencesToLC.GetByAncs(test)
		- allReferencesToLC.GetByAncs(increment);
		
	allLCRefInsideStmt = allLCRefInsideStmt - allLCRefInsideStmt.FindByType(typeof(Declarator));
	CxList operation = allLCRefInsideStmt.GetFathers();
		
	CxList assignment = allLCRefInsideStmt * leftSide;
		
	assignment.Add(allLCRefInsideStmt.FindByFathers(postfix.GetByAncs(operation).GetAncOfType(typeof(AssignExpr))));
		
	CxList change = postfix.FindByFathers(operation.FindByType(typeof(ExprStmt))) + //i++
		operation.FindByType(typeof(UnaryExpr)).FindByName("Increment") + //++i
		operation.FindByType(typeof(UnaryExpr)).FindByName("Decrement")  //--i		
		+assignment;
	
	change.Add(operation.FindByType(typeof(UnaryExpr)).FindByName("Address").GetAncOfType(typeof(MethodInvokeExpr)));
	result.Add(change);

}