CxList methods = Find_Methods();
CxList vars = All.FindByType(typeof(UnknownReference));
vars.Add(All.FindByType(typeof(Param)));

//Parameters
CxList dataSetParams = vars.FindByType("DataSet", false); 
CxList dataTableParams = vars.FindByType("DataTable", false);
CxList rowHandler = vars.FindByType("RowHandler", false);


// Fill(), FillSchema and Update DB methods for DataAdapter, IDataAdapter, IDbDataAdapter, 
// SqlDataAdapter, SqlCeDataAdapter, OdbcDataAdapter, OleDbDataAdapter and OracleDataAdapter
CxList fill = Find_DB_DataAdapter_Fill();
result.Add(fill);
result.Add(dataSetParams.GetParameters(fill));
result.Add(dataTableParams.GetParameters(fill));
result.Add(vars.FindByType("IDataReader", false).GetParameters(fill));

fill = Find_DB_DataAdapter_FillSchema();
result.Add(dataSetParams.GetParameters(fill));
result.Add(dataTableParams.GetParameters(fill));
result.Add(vars.FindByType("IDataReader", false).GetParameters(fill));
result.Add(fill);


//Update() DB methods for DataAdapter, IDataAdapter,
//IDbDataAdapter, SqlDataAdapter, SqlCeDataAdapter
//OdbcDataAdapter, OleDbDataAdapter and OracleDataAdapter
result.Add(Find_DB_DataAdapter_Update());

// ExecuteOracleNonQuery() for OracleCommand
CxList oracleNonQuery = methods.FindByMemberAccess("OracleCommand.ExecuteOracleNonQuery", false);
result.Add(vars.FindByType("OracleString", false).GetParameters(oracleNonQuery));


// Insert(), Update() and Delete() DB methods for SqlDataSource and AccessDataSource classes
// ExecuteReader() and ExecuteScalar() for IDbCommand, OdbcCommand, OleDbCommand,
// OracleCommand, SqlCommand, SqlCeCommand, EntityCommand classes, DB2Command, 
// UpdateCommand, InsertCommand, DeleteCommand and SelectCommand
// ExecuteXmlReader() for SqlCommand
// ExecuteOracleScalar() for OracleCommand
// Exec() and ExecBatch() for QSqlQuery
// Select() for SqlDataSource and AccessDataSource
// ExecuteXmlReader() and ExecuteResultSet() for DB2Command
// ExecuteDbDataReader() for UpdateCommand, InsertCommand, DeleteCommand and SelectCommand
CxList commandDSource = Find_DB_Command_DataSource_QSqlQuery();
result.Add(commandDSource);

// ExecuteResultSet for SqlCeCommand
result.Add(vars.FindByType("SqlCeResultSet", false).GetParameters(commandDSource));


// BeginExecuteReader(), BeginExecuteXmlReader() and BeginExecuteNonQuery () 
// for SqlCommand and SqlCeCommand
CxList beginExecReader = Find_DB_Command_BeginExecuteReader();
CxList beginExecReaderParams = vars.GetParameters(beginExecReader, 1);
result.Add(beginExecReaderParams);
result.Add(beginExecReader);


// EntLib
// ExecuteReader(), ExecuteDataSet() and ExecuteScalar() for DataBase, OracleDatabase,
// SqlDatabase, GenericDatabase, DB2Command, UpdateCommand, InsertCommand, DeleteCommand
// and SelectCommand
result.Add(Find_DB_EntLib_Execute());


// UpdateDataSet() for GenericDatabase, SqlDatabase, DataBase and OracleDatabase
result.Add(Find_DB_EntLib_Update());


//LoadDataSet() for DataBase, OracleDatabase, GenericDataBase and SqlDatabase
CxList entLib = Find_DB_EntLib_Load();
result.Add(dataSetParams.GetParameters(entLib));

// ExecuteNonQuery() methods for IDbCommand, OdbcCommand, OleDbCommand,
// OracleCommand, SqlCommand, SqlCeCommand, Db2Command, UdapteCommand, 
// DeleteCommand, InsertCommand and SelectCommand
// ExecuteOracleNonQuery() for OracleCommand
result.Add(Find_DB_Command_ExecuteNonQuery());

// ExecuteNonQuery() and DoExecuteNonQuery() for GenericDatabase, SqlDatabase,
// DataBase and OracleDatabase
result.Add(Find_DB_EntLib_ExecuteNonQuery());

//ExecuteAndSend() method for SqlPipe
result.Add(methods.FindByMemberAccess("SqlPipe.ExecuteAndSend", false).GetTargetOfMembers());

//Ibatis
//QueryForObject(), QueryForList(), QueryForMap(), Insert(), Update() and
//Delete() and QueryWithRowHandler() methods for SqlMapper
CxList ibatis = methods.FindByMemberAccess("SqlMapper.QueryWithRowHandler", false);
result.Add(Find_DB_Ibatis());
result.Add(rowHandler.GetParameters(ibatis));


// Salesforce
// Query(), QueryAll() and Search() methods for SforceService
result.Add(Find_DB_Salesforce());


//EntityFramework
result.Add(Find_DB_EF_Out());


//Linq
result.Add(Find_DB_Linq_Out());


//Xamarin
result.Add(Find_DB_Sqlite_Xamarin());