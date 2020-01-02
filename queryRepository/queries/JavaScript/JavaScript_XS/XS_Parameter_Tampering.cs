//this query will look for flow from input to db-in , when query satement does not have an "and" statement
//in case no "and" statement (or multiple query fields of XSDS) exist , a parameter tampering vulnerability is possible
//over the SQL query
//a heuristic sanitizer will be flow that goes through "if" statement- assumes some check is done

if(cxScan.IsFrameworkActive("XSJS"))
{
	//db and hdb section
	CxList xsAll = XS_Find_All();
	CxList dbIn = XS_Find_DB_In();	
	CxList sl = xsAll.FindByType(typeof(StringLiteral));
	
	//only work on select update or delete statement
	CxList queryStmt = sl.FindByShortName("*select *", false);
	queryStmt.Add(sl.FindByShortName("*update *", false));
	queryStmt.Add(sl.FindByShortName("*delete *", false));
	
	//look for where and and statements
	CxList whereStmt = sl.FindByShortName("*where *", false);
	CxList andStmt = sl.FindByShortName("*and *", false);
	
	//find all inputs for the source of the flow
	CxList inputs = XS_Find_Interactive_Inputs();
	
	//sanitizer
	CxList ifStmt = xsAll.FindByType(typeof(IfStmt));
	CxList underIfConditionSanitizer = (xsAll - xsAll.FindByType(typeof(StatementCollection))).FindByFathers(ifStmt);
	CxList sanitize = xsAll.GetByAncs(underIfConditionSanitizer);
	
	//we will look only for db- in that are select,update and delete that have "where" statements and don't have "and"
	//statements
	CxList relevantDBIn = dbIn.DataInfluencedBy(queryStmt) + (dbIn * queryStmt);
	CxList dbInWithWhere = (dbIn.DataInfluencedBy(whereStmt)) + (dbIn * whereStmt);
	CxList dbWithAnd = (dbIn.DataInfluencedBy(andStmt)) + (dbIn * andStmt);
	dbInWithWhere -= dbWithAnd;
	relevantDBIn = dbInWithWhere * relevantDBIn;
	
	//first type of flow.
	CxList dbMethods = xsAll.FindByType(typeof(MethodInvokeExpr)).FindByParameters(relevantDBIn);
	CxList allDBMethodsParameters = dbIn.GetParameters(dbMethods);
	
	result = allDBMethodsParameters.InfluencedByAndNotSanitized(inputs, sanitize);
	
	
	//now we work on hdb setters, flow that goes from input to a setter is not safe either, under the same conditions as
	//in previous section
	CxList SQLSetter = XS_Find_DB_Setters();
	SQLSetter = SQLSetter.DataInfluencedBy(relevantDBIn);
	result.Add(SQLSetter.InfluencedByAndNotSanitized(inputs, sanitize + result));
	
	
	//XSDS section
	
	CxList XSdsDBIn = XSDS_Find_DB_In();
	//XSDS_Find_DB_In() gives us the parameter, we need to get back to the invocation
	XSdsDBIn = xsAll.FindByParameters(XSdsDBIn);
	List<string> names = new List<string>(new string[]{"$find*","$get","$select","$delete"});
	//let's work only on Entity object
	CxList dbInvokes = XSdsDBIn.FindByShortNames(names);
	//$get is a single where- so is always vulnerable
	CxList xsdsGet = dbInvokes.FindByShortName("$get");
	result.Add(xsdsGet.InfluencedByAndNotSanitized(inputs, sanitize));
	dbInvokes -= xsdsGet;
	
	//for the rest we will look whether the basic codition statement has more than two keys
	CxList conditions = xsAll.GetParameters(dbInvokes, 0);

	//if it has only a single field- potentially vulnerable to parameter tampering
	result.Add(conditions.InfluencedByAndNotSanitized(inputs, sanitize));
	
	//QueryObj
	//we will look fo $where statement that is not influenced by $and statement 
	CxList query = XSdsDBIn.FindByShortName("$where");
	query -= query.DataInfluencedBy(xsAll.FindByShortName("$and"));

	CxList queryConditions = All.NewCxList();
	queryConditions.Add(query);
	queryConditions.Add(conditions);
		
	result.Add(queryConditions.InfluencedByAndNotSanitized(inputs, sanitize));
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}