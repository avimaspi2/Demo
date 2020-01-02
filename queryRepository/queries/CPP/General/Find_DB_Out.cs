CxList methods = Find_Methods();

result = methods.FindByMemberAccesses(new string[]{"Statement.execute*",
	"SACommand.Execute*",
	"_CommandPtr.Execute*",
	"Command.Execute*",
	"Connection.Execute*",
	"CDaoQueryDef.Execute",
	"Statement.execute"});

result.Add(methods.FindByShortNames(new List<string>(){"OCIStmtExecute",
		"SQLExecute"		,"mysql_query",
		"mysql_real_query"	,"CxESQL_select" }));

result.Add(methods.FindByMemberAccesses(new string[]{
	"_RecordsetPtr.Open*",
	"Recordset.Open*",
	"CDaoRecordset.Open"}));

result.Add(All.GetParameters(methods.FindByShortName("SQLExecDirect"), 1));


result.Add(methods.FindByMemberAccesses(new string[]{
	"SqlDataSource.Select",

	"AccessDataSource.Select",

	"IDbCommand.ExecuteReader",
	"IDbCommand.ExecuteScalar",
	"IDbCommand.ExecuteNonQuery",

	"OdbcCommand.ExecuteReader",
	"OdbcCommand.ExecuteScalar",
	"OdbcCommand.ExecuteNonQuery",

	"OleDbCommand.ExecuteReader",
	"OleDbDataReader.ExecuteReader",
	"OleDbCommand.ExecuteScalar",
	"OleDbCommand.ExecuteNonQuery"}));

List<string> OracleCommandMethods = new List<string> {
		"ExecuteReader", "ExecuteScalar", "ExecuteNonQuery",
		"ExecuteOracleNonQuery", "ExecuteOracleScalar"
		};

result.Add(methods.FindByMemberAccess("OracleCommand").GetMembersOfTarget().FindByShortNames(OracleCommandMethods));

List<string> SqlCommandMethods = new List<string> {
		"ExecuteReader", "ExecuteScalar", "ExecuteNonQuery", "ExecuteXmlReader",
		"BeginExecuteReader", "BeginExecuteNonQuery", "BeginExecuteXmlReader"
		};

result.Add(methods.FindByMemberAccess("SqlCommand").GetMembersOfTarget().FindByShortNames(SqlCommandMethods));

List<string> SqlCeCommandMethods = new List<string> {
		"ExecuteReader", "ExecuteResultSet", "ExecuteScalar", "ExecuteNonQuery",
		"ExecuteXmlReader", "BeginExecuteReader", "BeginExecuteNonQuery", "BeginExecuteXmlReader"
		};

result.Add(methods.FindByMemberAccess("SqlCeCommand").GetMembersOfTarget().FindByShortNames(SqlCeCommandMethods));

result.Add(methods.FindByMemberAccesses(new string[]{
	"SelectCommand.Execute*",

	"SqlPipe.ExecuteAndSend",

	"DB2Command.Execute*",

	"QSqlQuery.exec",
	"QSqlQuery.execBatch",

	"mysqlpp.Query.store*",
	"Query.store*",

	"mysqlpp.Query.exec*",
	"Query.exec*",

	"mysqlpp.Query.for_each*",
	"Query.for_each*",

	"mysqlpp.Query.use*",
	"Query.use*",

	"mysqlpp.DBDriver.execute",
	"DBDriver.execute"}));

result.Add(Find_DB_PostgreSQL_Outputs());