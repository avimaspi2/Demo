CxList methods = Find_Methods();

result = methods.FindByMemberAccesses(new string[]{"Statement.execute*", //occi
	"SACommand.Execute*", //sqlapi
	"_CommandPtr.Execute*", //adodb
	"Command.Execute*", //adodb
	"Connection.Execute*", //adodb
	"_RecordsetPtr.Open*", //adodb
	"Recordset.Open*"});//adodb

result.Add(
	methods.FindByMemberAccesses(new string[]{"CDaoRecordset.Open", "CDaoQueryDef.Execute", "Statement.execute"}) +
	methods.FindByShortName("OCIStmtExecute") +
	methods.FindByShortName("SQLExecute") +
	All.GetParameters(methods.FindByShortName("SQLExecDirect"), 1) + // odbc
	All.GetParameters(methods.FindByShortName("SQLPutData"), 1));

result.Add(
	methods.FindByMemberAccesses(new string[]{
	"AccessDataSource.Delete",
	"AccessDataSource.Insert",
	"AccessDataSource.Update",

	"DB2Command.Execute*",

	"DbDataAdapter.Fill",
	"DbDataAdapter.FillSchema",
	"DbDataAdapter.Update",

	"DeleteCommand.Execute*",
	"InsertCommand.Execute*",
	"UpdateCommand.Execute*",

	"IDataAdapter.Fill",
	"IDataAdapter.FillSchema", 
	"IDataAdapter.Update",

	"IDbCommand.ExecuteNonQuery",
	"IDbCommand.ExecuteScalar",

	"IDbDataAdapter.Fill",
	"IDbDataAdapter.FillSchema",
	"IDbDataAdapter.Update",

		
	"mysqlpp.Query.exec*",
	"mysqlpp.Query.execute",
	"mysqlpp.Query.for_each*",
	"mysqlpp.Query.store*",
	"mysqlpp.Query.use*",

	"OdbcCommand.ExecuteNonQuery",
	"OdbcCommand.ExecuteScalar",

	"OdbcDataAdapter.Fill",
	"OdbcDataAdapter.FillSchema",
	"OdbcDataAdapter.Update",

	"OleDbCommand.ExecuteNonQuery",
	"OleDbCommand.ExecuteScalar",

	"OleDbDataAdapter.Fill",
	"OleDbDataAdapter.FillSchema",
	"OleDbDataAdapter.Update",

	"OracleCommand.ExecuteNonQuery",
	"OracleCommand.ExecuteOracleNonQuery",
	"OracleCommand.ExecuteOracleScalar",
	"OracleCommand.ExecuteScalar",

	"OracleDataAdapter.Fill",
	"OracleDataAdapter.FillSchema",
	"OracleDataAdapter.Update",

	"QSqlQuery.exec",
	"QSqlQuery.execBatch",

	"SqlCeCommand.BeginExecuteNonQuery",
	"SqlCeCommand.BeginExecuteReader",
	"SqlCeCommand.BeginExecuteXmlReader",
	"SqlCeCommand.ExecuteNonQuery",
	"SqlCeCommand.ExecuteResultSet",
	"SqlCeCommand.ExecuteScalar",
	"SqlCeCommand.ExecuteXmlReader",

	"SqlCeDataAdapter.Fill",
	"SqlCeDataAdapter.FillSchema",
	"SqlCeDataAdapter.Update",

	"SqlCommand.BeginExecuteNonQuery",
	"SqlCommand.BeginExecuteReader",
	"SqlCommand.BeginExecuteXmlReader",
	"SqlCommand.ExecuteNonQuery",
	"SqlCommand.ExecuteScalar",
	"SqlCommand.ExecuteXmlReader",

	"SqlDataAdapter.Fill",
	"SqlDataAdapter.FillSchema",
	"SqlDataAdapter.Update",

	"SqlDataSource.Delete",
	"SqlDataSource.Insert",
	"SqlDataSource.Update",

	"SqlPipe.ExecuteAndSend"
	}));

result.Add(Find_ESQL_DB());
result.Add(Find_DB_PostgreSQL_Inputs());