//Zend XSS sanitizers

if (Find_Extend_Zend().Count > 0)
{
	CxList methods = Find_Methods();
	CxList getValuesMethod = 
		methods.FindByShortName("getValue*") + 
		methods.FindByShortName("getUnfilteredValue*");
	

	CxList output = Find_Interactive_Outputs();

	CxList getValidValueMethod = methods.FindByShortName("getValidValue*");
	CxList isValidMethods = methods.FindByShortName("isValid");
	CxList filter = methods.FindByShortName("filter");


	CxList ifStmt = (All.FindByType(typeof(IfStmt)));
	CxList exp = All.FindByType(typeof(Expression));
	CxList isValidMethodAsCond = isValidMethods.GetByAncs(exp.FindByFathers(ifStmt));

	CxList relevantIfStmt = isValidMethodAsCond.GetAncOfType(typeof(IfStmt));

	CxList getValuesMethodInRelevantIf = All.NewCxList();
	CxList obj = All.NewCxList();
	CxList temp = All.NewCxList();


	exp = exp.FindByFathers(relevantIfStmt);
	CxList allSingleIfChilds = All.FindByFathers(relevantIfStmt);
	foreach (CxList singleIf in relevantIfStmt)
	{		
		CxList singleIfChilds = allSingleIfChilds.FindByFathers(singleIf);
		obj = isValidMethods.GetByAncs(exp.FindByFathers(singleIf)).GetTargetOfMembers();
		temp = getValuesMethod.GetByAncs(singleIfChilds).GetTargetOfMembers();
		if (temp.FindAllReferences(obj).Count > 0)
		{
			getValuesMethodInRelevantIf.Add(getValuesMethod.GetByAncs(singleIfChilds));
		}
	}


	// Not is valid 
	CxList isNOTValidMethodAsCond = isValidMethodAsCond.GetByAncs(All.FindByShortName("Not").FindByType(typeof(UnaryExpr)));
	CxList funcDeclWithNOTValidMethodAsCond = isNOTValidMethodAsCond.GetAncOfType(typeof(MethodDecl));


	CxList inIf = All.GetByAncs(relevantIfStmt - isNOTValidMethodAsCond.GetAncOfType(typeof(IfStmt)) - getValuesMethod.GetAncOfType(typeof(IfStmt)));
	CxList sanitizedDB = All.NewCxList();
	foreach (CxList oneInIf in inIf)
	{
		CxList cls = oneInIf.GetAncOfType(typeof(ClassDecl));
		sanitizedDB.Add(oneInIf.DataInfluencingOn(output.GetByAncs(cls)));
	}

	result = getValuesMethodInRelevantIf + getValuesMethod.GetByAncs(funcDeclWithNOTValidMethodAsCond) - getValuesMethod.GetByAncs(isNOTValidMethodAsCond.GetAncOfType(typeof(IfStmt))) + 
		getValidValueMethod + 
		filter + 
		sanitizedDB;
}