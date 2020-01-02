CxList _params = Find_Parameters();
CxList uRef = Find_UnknownReference();
CxList mA = Find_MemberAccesses();
CxList urMA = uRef.Clone();
urMA.Add(mA);
CxList sanitize = All.NewCxList();

//add all input to db mentods - methods like: insert(), update()
sanitize.Add(NodeJS_DB_Input_Methods());

//add all output from db methods - methods like: get(), fetch()
sanitize.Add(NodeJS_DB_Output_Methods());

////////////////////////////////////////////////////////////////////////////////////
//add input to db methods for db-mysql DB
CxList requireOfDBmysql = Find_Require("db-mysql");

List<String> allFilesWithDBmysql = new List<String>();
CxList allINSQLDBmysql = All.NewCxList();
foreach(CxList reqDB in requireOfDBmysql)
{
	try
	{
		CSharpGraph reqDBGR = reqDB.GetFirstGraph();
		String rDBName = reqDBGR.LinePragma.FileName;
		if (!allFilesWithDBmysql.Contains(rDBName))
		{
			allFilesWithDBmysql.Add(rDBName);
			allINSQLDBmysql.Add(All.FindByFileName(rDBName));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList methInvSQLDBmysql = allINSQLDBmysql.FindByType(typeof (MethodInvokeExpr));

CxList sqlQueryDBmysql = methInvSQLDBmysql.FindByShortNames(new List<string>{"delete","insert","select","update"});

sanitize.Add(sqlQueryDBmysql);
////////////////////////////////////////////////////////////////////////////////////

//Add all parameters of sanitation methods to sanitizers
_params.Add(uRef);
CxList sanitizeParams = _params.GetParameters(sanitize);
sanitize.Add(urMA.GetByAncs(sanitizeParams));
////////////////////////////////////////////////////////////////////////////////////

result.Add(sanitize);
result.Add(NodeJS_Find_General_Sanitize());
result.Add(NodeJS_Find_Sql_Sanitize());
result.Add(NodeJS_Find_Sqlite_sanitizers());

// Sanitize usage of parameterized queries in query/execute calls in mysql/couchbase.
// If we have a call with more than two parameters, the second one is a 
//    value/object of values to be bind on the query (ex: con.query("Select ?", input, cb);)
CxList queryMethods = NodeJS_Find_DB_Base().FindByShortNames(new List<string> {"query", "execute"});
CxList thirdParam = All.GetParameters(queryMethods, 2);
CxList queryMethodWithThreeParameters = thirdParam.GetAncOfType(typeof(MethodInvokeExpr));
// Add second parameter as sanitizer
result.Add(All.GetParameters(queryMethodWithThreeParameters, 1));