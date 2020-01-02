//Zend sanitizers


if (Find_Extend_Zend().Count > 0)
{
	CxList methods = Find_Methods();
	CxList getValuesMethod = methods.FindByShortNames(new List<String>(){ "getValue*", "getUnfilteredValue*" });
	
	CxList db = Find_Zend_DB_In();

	CxList getValidValueMethod = methods.FindByShortName("getValidValue*");
	CxList isValidMethods = methods.FindByShortName("isValid");
	CxList filter = methods.FindByShortName("filter");
	CxList escape = methods.FindByShortName("escape");

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
		sanitizedDB.Add(oneInIf.DataInfluencingOn(db.GetByAncs(cls)));
	}

	result = getValuesMethodInRelevantIf + getValuesMethod.GetByAncs(funcDeclWithNOTValidMethodAsCond) - getValuesMethod.GetByAncs(isNOTValidMethodAsCond.GetAncOfType(typeof(IfStmt))) + 
		getValidValueMethod + 
		filter + 
		escape + sanitizedDB;

	
	//Zend DB sanitizers:
	
	CxList methodsParams = All.GetParameters(methods);

	

	CxList directDbMethods = methods.FindByShortNames(new List<string> {
			//Zend_Db_Adapter
			"delete",  			//internally quote params
			"insert",  			//insert is sanitized because it internally uses prepared statements
			"lastInsertId",  	//safely implemented in Oracle, DB2, PostgreSQL. 
			"lastSequenceId", 	
			"nextSequenceId", 
			"quote", 
			"quoteColumnAs", 
			"quoteIdentifier", 
			"quoteInto", 
			"quoteTableAs", 
			"update",			//internally quote params
	
			//Zend_Db_Select
			"bind",

			//Zend_Db_Statement
			"bindColumn",  
			"bindParam",  
			"bindValue", 
			"execute"}); 			//execute is sanitized because is binded to prepared statement
	/*
	CxList directDbMethods = 
	methods.FindByShortName("delete") + //internally quote params
	methods.FindByShortName("insert") + //insert is sanitized because it internally uses prepared statements
	methods.FindByShortName("lastInsertId") + //safely implemented in Oracle, DB2, PostgreSQL. 
	methods.FindByShortName("lastSequenceId") +	
	methods.FindByShortName("nextSequenceId") +
	methods.FindByShortName("quote") +
	methods.FindByShortName("quoteColumnAs") +
	methods.FindByShortName("quoteIdentifier") +
	methods.FindByShortName("quoteInto") +
	methods.FindByShortName("quoteTableAs") +
	methods.FindByShortName("update");//internally quote params
	
	//Zend_Db_Select
	directDbMethods.Add(
	methods.FindByShortName("bind"));


	//Zend_Db_Statement
	directDbMethods.Add(
	methods.FindByShortName("bindColumn") + 
	methods.FindByShortName("bindParam") + 
	methods.FindByShortName("bindValue") +
	methods.FindByShortName("execute")); //execute is sanitized beacuase is binded to prepared statement
	*/

	//invulnerable parameters in ambiguous functions

	CxList invulnerableParam = 
		
		methodsParams.GetParameters(methods.FindByShortName("having"), 1) +  
		methodsParams.GetParameters(methods.FindByShortName("limit"), 1) +  
		methodsParams.GetParameters(methods.FindByShortName("limit"), 2) +
		methodsParams.GetParameters(methods.FindByShortName("orHaving"), 1) + 
		methodsParams.GetParameters(methods.FindByShortName("orWhere"), 1) + 
		methodsParams.GetParameters(methods.FindByShortName("query"), 1);
	
	invulnerableParam.Add(methodsParams.GetParameters(methods.FindByShortName("where"), 1) - Find_Doctrine_Where_Non_Sanitizer());
	
	result.Add(directDbMethods + invulnerableParam);
}