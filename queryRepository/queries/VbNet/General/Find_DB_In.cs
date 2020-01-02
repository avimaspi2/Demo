CxList methods = Find_Methods();
CxList vars = All.FindByType(typeof(UnknownReference));
vars.Add(All.FindByType(typeof(Param)));

//Parameters
CxList dataSetParams = vars.FindByType("DataSet", false); 
CxList dataRowParams = vars.FindByType("DataRow", false);
CxList dataTableParams = vars.FindByType("DataTable", false);
CxList strings = vars.FindByType(typeof(StringLiteral));
CxList dbCommand = vars.FindByType("DbCommand", false);


// Fill(), FillSchema and Update DB methods for DataAdapter, IDataAdapter, IDbDataAdapter, 
// SqlDataAdapter, SqlCeDataAdapter, OdbcDataAdapter, OleDbDataAdapter and OracleDataAdapter
CxList fill = Find_DB_DataAdapter_Fill();
result.Add(fill.GetTargetOfMembers());
result.Add(strings.GetParameters(fill));
result.Add(Find_DB_DataAdapter_FillSchema().GetTargetOfMembers());


//Update() DB methods for DataAdapter, IDataAdapter,
//IDbDataAdapter, SqlDataAdapter, SqlCeDataAdapter
//OdbcDataAdapter, OleDbDataAdapter and OracleDataAdapter
result.Add(All.GetParameters(Find_DB_DataAdapter_Update()));


// Insert(), Update() and Delete() DB methods for SqlDataSource and AccessDataSource classes
// ExecuteReader() and ExecuteScalar() for IDbCommand, OdbcCommand, OleDbCommand,
// OracleCommand, SqlCommand, SqlCeCommand, EntityCommand classes, DB2Command, 
// UpdateCommand, InsertCommand, DeleteCommand and SelectCommand
// ExecuteXmlReader() for SqlCommand
// ExecuteOracleScalar() for OracleCommand
// ExecuteResultSet for SqlCeCommand
// ExecuteXmlReader() and ExecuteResultSet() for DB2Command
// ExecuteDbDataReader() for UpdateCommand, InsertCommand, DeleteCommand and SelectCommand
CxList commandDSource = Find_DB_Command_DataSource_QSqlQuery();
result.Add(commandDSource.GetTargetOfMembers());

//Exec() and ExecBatch() for QSqlQuery
result.Add(vars.FindByType("QString", false).GetParameters(commandDSource));

//Select() for SqlDataSource and AccessDataSource
CxList selectOnly = commandDSource.FindByShortName("Select", false);
result.Add(vars.FindByType("DataSourceSelectArguments", false).GetParameters(selectOnly));
	

// ExecuteNonQuery() methods for IDbCommand, OdbcCommand, OleDbCommand,
// OracleCommand, SqlCommand, SqlCeCommand, Db2Command, UdapteCommand, 
// DeleteCommand, InsertCommand and SelectCommand
// ExecuteOracleNonQuery() for OracleCommand
CxList executeNonQuery = Find_DB_Command_ExecuteNonQuery();
result.Add(executeNonQuery.GetTargetOfMembers());

// BeginExecuteReader(), BeginExecuteXmlReader() and
// BeginExecuteNonQuery() for SqlCommand and SqlCeCommand
result.Add(Find_DB_Command_BeginExecuteReader().GetTargetOfMembers());


// EntLib
// ExecuteReader(), ExecuteDataSet() and ExecuteScalar() for DataBase, OracleDatabase,
// SqlDatabase, GenericDatabase, 
//DB2Command, UpdateCommand, InsertCommand, DeleteCommand and SelectCommand
CxList entLib = Find_DB_EntLib_Execute();
result.Add(strings.GetParameters(entLib));
result.Add(dbCommand.GetParameters(entLib));
//Object parameters
CxList obj = vars.GetParameters(entLib, 1);
obj.Add(vars.GetParameters(entLib, 2));
obj -= obj.FindByType("DbTransaction", false);
obj -= obj.FindByType("CommandType", false);
result.Add(obj);


// UpdateDataSet() for GenericDatabase, SqlDatabase, DataBase and OracleDatabase
entLib = Find_DB_EntLib_Update();
result.Add(strings.GetParameters(entLib));
result.Add(dbCommand.GetParameters(entLib));


//LoadDataSet() for DataBase, OracleDatabase, GenericDataBase and SqlDatabase
entLib = Find_DB_EntLib_Load();
result.Add(strings.GetParameters(entLib));
result.Add(dbCommand.GetParameters(entLib));
//objects parameters
obj = vars.GetParameters(entLib, 3);
obj.Add(vars.GetParameters(entLib, 4));
obj -= obj.FindByType("DbTransaction", false);
obj -= obj.FindByType("DataSet", false);
result.Add(obj);

// ExecuteNonQuery() and DoExecuteNonQuery() for GenericDatabase, SqlDatabase,
// DataBase and OracleDatabase
CxList doExecuteNonQuery = Find_DB_EntLib_ExecuteNonQuery();
result.Add(strings.GetParameters(doExecuteNonQuery));
result.Add(dbCommand.GetParameters(doExecuteNonQuery));
//objects parameters
obj = vars.GetParameters(doExecuteNonQuery, 1);
obj.Add(vars.GetParameters(doExecuteNonQuery, 2));
obj -= obj.FindByType("DbTransaction", false);
obj -= obj.FindByType("CommandType", false);
result.Add(obj);

//ExecuteAndSend() method for SqlPipe
CxList sqlPipe = methods.FindByMemberAccess("SqlPipe.ExecuteAndSend", false);
result.Add(vars.FindByType("SqlCommand", false).GetParameters(sqlPipe));


//Ibatis
//QueryForObject(), QueryForList(), QueryForMap(), Insert(), Update() and
//Delete() and QueryWithRowHandler() methods for SqlMapper
CxList ibatis = Find_DB_Ibatis();
ibatis.Add(methods.FindByMemberAccess("SqlMapper.QueryWithRowHandler", false));
result.Add(strings.GetParameters(ibatis));
//objects parameters
obj = vars.GetParameters(ibatis, 1);
obj -= obj.FindByType("RowHandler", false);
result.Add(obj);


// Salesforce
// Query(), QueryAll() and Search() methods for SforceService
CxList salesforces = Find_DB_Salesforce();
result.Add(strings.GetParameters(salesforces));

//EntityFramework
result.Add(Find_DB_EF_In());

//Linq
result.Add(Find_DB_Linq_In());

//Xamarin
result.Add(All.GetParameters(Find_DB_Sqlite_Xamarin()));