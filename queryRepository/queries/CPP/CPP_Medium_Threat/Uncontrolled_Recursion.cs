CxList methodDecls = Find_MethodDecls();
CxList methods = Find_Methods();
methods.Add(Find_UnknownReference());
CxList methInv = All.FindAllReferences(methodDecls).FindByType(typeof(MethodInvokeExpr));
CxList conds = Find_Ifs();
conds.Add(Find_IterationStmt());
conds.Add(Find_Cases());
CxList returns = Find_ReturnStmt();

foreach (CxList method in methodDecls)
{
	
	// Recursive calls inside a lambda expression are considered controlled
	if(method.GetName() == "Lambda")
		continue;
	
	// Fetch all the recursive calls, if any.
	CxList curInvokes = methInv.GetByAncs(method);
	CxList recursiveCalls = curInvokes.FindAllReferences(method);

	if (recursiveCalls.Count == 0)
		continue;
	
	// Compares the name of the method with the name of the supposed recursive function
	if(recursiveCalls.GetLeftmostTarget().Count != 0)
		if(method.GetAncOfType(typeof(ClassDecl)).GetName() != recursiveCalls.GetLeftmostTarget().GetName())
			continue;
	
	// Recursive calls inside an conditions or return statements are *heuristically* considered controlled.
	CxList methodConds = conds.GetByAncs(method);
	CxList methodReturns = returns.GetByAncs(method);
	methodConds.Add(methodReturns);
	recursiveCalls -= recursiveCalls.GetByAncs(methodConds);
	
	//in case one of conditions contains return statement this case *heuristically* will be considered as controlled.
	if (recursiveCalls.Count == 0 && (methodConds.Count == 0 || methodReturns.Count == 0))
		continue;
	CxList returnsInCond = methodReturns.GetByAncs(conds);
	if (returnsInCond.Count > 0)
		continue;
	
	result.Add(recursiveCalls);
}