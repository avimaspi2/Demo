/* MISRA CPP RULE 6-5-4
 ------------------------------
 This query finds all for loops that the incremental part is of a form that has an assignment of n and n is being modified
 inside the for.

 The following example shows what main code should look like: 
     
     for(int j=0; j<100; j+=foo());     //non-compliant  
     
     for(int k=0; k<180;k+=*p)          //non-compliant
     { 
        bar(p);
     }

	 for (int i = 0; i < 100; i+= n)  //non-compliant
	 {
	   	n++;
	   	n+= 10;
	    n = n * n;
	    
	 }

*/

CxList allFors = All.FindByType(typeof(IterationStmt));

CxList helper = allFors;
foreach(CxList allf in allFors)
{
	IterationStmt i = allf.data.GetByIndex(0) as IterationStmt;
	if(i != null){
		IterationType it = i.IterationType;
		if(!it.ToString().Equals("For")){
			helper -= allf;
		}
	}
}
allFors = helper;


CxList unrf = All.FindByType(typeof(UnknownReference)).GetByAncs(allFors);
CxList declarators = Find_All_Declarators().GetByAncs(allFors);
CxList leftSd = unrf.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList binEx = All.FindByType(typeof(BinaryExpr)).GetByAncs(allFors);
CxList methInv = All.FindByType(typeof(MethodInvokeExpr)).GetByAncs(allFors);
CxList unaryEx = All.FindByType(typeof(UnaryExpr)).GetByAncs(allFors);
CxList rightSd = All.FindByAssignmentSide(CxList.AssignmentSide.Right);

//find the init part of the for statement

foreach(CxList cur in allFors)
{
	CxList thisRef = unrf.GetByAncs(cur);
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
	
	CxList unknownRef = thisRef.GetByAncs(init);
	
	
	//retrieves the loop counters
	CxList loopCounter = unknownRef.FindByFathers(init.FindByType(typeof(ExprStmt)));

	CxList leftAsn = unknownRef * leftSd;
	loopCounter.Add(leftAsn + declarators.GetByAncs(init));

	//retrieves the increment part of the loop
	CxList increment = All.NewCxList();
	StatementCollection incrementColl = iterA.Increment;
	for(int i = 0;  incrementColl != null && i < incrementColl.Count; i++)
	{	
		if(incrementColl[i] != null)
		{
			increment.Add(All.FindById(incrementColl[i].NodeId));
		}
	}
	
	CxList test = All.NewCxList();
	Expression testExp = iterA.Test;
	if(testExp != null)
	{
		test.Add(All.FindById(testExp.NodeId));
	}
	
	
	CxList testUn = thisRef.GetByAncs(test);
	CxList incrUn = thisRef.GetByAncs(increment);
	CxList additionalLC = incrUn.FindAllReferences(testUn) + testUn.FindAllReferences(incrUn);
	loopCounter.Add(additionalLC);

	//find all illegal for expressions
	CxList lcInIncrement = incrUn.FindAllReferences(loopCounter);
	result.Add(lcInIncrement.GetFathers().FindByType(typeof(BinaryExpr)).GetAncOfType(typeof(IterationStmt)));
	
	CxList oper = lcInIncrement.GetFathers() - binEx.FindByType(typeof(BinaryExpr));
	
	CxList asnInIncrement = oper.FindByType(typeof(AssignExpr));
	
	CxList lc = thisRef.FindAllReferences(loopCounter);

	CxList assign = (lc.FindByFathers(asnInIncrement) * leftSd).GetFathers();
	CxList totalResult = All.NewCxList();
	foreach(CxList a in assign)
	{
		AssignExpr ae = a.data.GetByIndex(0) as AssignExpr;	
		if(ae != null)
		{
			AssignOperator o = ae.Operator;
			if(o != null)
			{
				String opertr = o.ToString();
				if(!opertr.Equals("AdditionAssign") && !opertr.Equals("SubtractionAssign"))
				{
					assign -= All.FindById(ae.NodeId);
					totalResult.Add(All.FindById(ae.NodeId));
				}
			}
		}
	}
	result.Add(totalResult.GetAncOfType(typeof(IterationStmt)));
	asnInIncrement = assign;
	
	CxList temp = methInv.GetByAncs(increment);
	CxList methInvInIncr = temp * rightSd;

	result.Add(methInvInIncr.GetAncOfType(typeof(IterationStmt)));
	asnInIncrement -= methInvInIncr.GetAncOfType(typeof(AssignExpr));
	result.Add((temp - methInvInIncr).GetAncOfType(typeof(IterationStmt)));

	//asnInIncrement now has all expressions of the increment part that are of the form -= +=
	//find all right side of the -= +=
	CxList allRefsInIncrement = thisRef.GetByAncs(assign);
	CxList pointer = unaryEx.FindByName("Pointer");
	CxList rightSide = allRefsInIncrement * rightSd;
	
	CxList rtPtr = pointer * rightSd;
	
	CxList pointerRefInIncr = allRefsInIncrement.FindByFathers(rtPtr);
	
	CxList allRefToRs = All.NewCxList();
	CxList change = All.NewCxList();
	
	CxList curExpr = thisRef.GetByAncs(rightSide.GetAncOfType(typeof(IterationStmt)));
	
	allRefToRs = curExpr.FindAllReferences(rightSide) - rightSide;//+=
	CxList allRefsInStmt = allRefToRs - thisRef.GetByAncs(increment) - thisRef.GetByAncs(init);
	CxList operation = allRefsInStmt.GetFathers();
	CxList edit = operation.FindByType(typeof(ExprStmt)) +
		operation.FindByType(typeof(UnaryExpr)).FindByName("Increment") +
		operation.FindByType(typeof(UnaryExpr)).FindByName("Decrement");
	edit.Add(allRefsInStmt.FindByAssignmentSide(CxList.AssignmentSide.Left));
	edit.Add(operation.FindByType(typeof(UnaryExpr)).FindByName("Address").GetAncOfType(typeof(MethodInvokeExpr)));
	
	
	if(edit.Count > 0)
	{
		result.Add(cur);
	}
	CxList thisExpr = thisRef.GetByAncs(pointerRefInIncr.GetAncOfType(typeof(IterationStmt)));
	allRefToRs = thisExpr.FindAllReferences(pointerRefInIncr) - pointerRefInIncr;
	CxList allRefsStmt = allRefToRs - thisRef.GetByAncs(increment) - thisRef.GetByAncs(init);
	CxList prm = allRefsStmt.GetFathers().FindByType(typeof(Param));
	if(prm.Count > 0)
	{
		result.Add(cur);
	}
	CxList ptrToRf = allRefsStmt.GetFathers().FindByType(typeof(UnaryExpr)).FindByName("Pointer");
	CxList ris = thisRef.FindByFathers(ptrToRf).GetFathers();
	operation = ris.GetFathers();
	edit = operation.FindByType(typeof(ExprStmt)) +
		operation.FindByType(typeof(UnaryExpr)).FindByName("Increment") +
		operation.FindByType(typeof(UnaryExpr)).FindByName("Decrement");
	edit.Add(ris.FindByAssignmentSide(CxList.AssignmentSide.Left));
	
	if(edit.Count > 0)
	{
		result.Add(cur);
	}
	
}