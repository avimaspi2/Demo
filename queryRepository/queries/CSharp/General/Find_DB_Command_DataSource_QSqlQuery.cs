/* Insert(), Select(), Update() and Delete() DB methods for SqlDataSource and AccessDataSource classes
 * ExecuteReader() and ExecuteScalar() for IDbCommand, OdbcCommand, OledbCommand,
 * OracleCommand, SqlCommand, SqlCeCommand, EntityCommand classes, DB2Command, 
 * UpdateCommand, InsertCommand, DeleteCommand, SelectCommand, 
 * SqliteCommand and SqliteConnections
 * ExecuteXmlReader() for SqlCommand
 * ExecuteOracleScalar() for OracleCommand
 * Exec() and ExecBatch() for QSqlQuery
 * Select() for SqlDataSource and AccessDataSource
 * ExecuteResultSet for SqlCeCommand
 * ExecuteXmlReader() and ExecuteResultSet() for DB2Command
 * ExecuteDbDataReader() for UpdateCommand, InsertCommand, DeleteCommand and SelectCommand
 */

CxList methods = Find_Methods();

//SqlDataSource and AccessDataSource

result = methods.FindByMemberAccesses(new string[]{
	"SqlDataSource.Update",
	"SqlDataSource.Insert",
	"SqlDataSource.Delete",
	"SqlDataSource.Select"});
result.Add(All.FindByMemberAccess("SqlDataSource.SelectCommand"));

result.Add(methods.FindByMemberAccesses(new string[]{
	"AccessDataSource.Update",
	"AccessDataSource.Insert",
	"AccessDataSource.Delete",
	"AccessDataSource.Select",

	//IDbCommand, OdbcCommand, OleDbCommand, OracleCommand, 
	//EntityCommand, SqlCommand, SqlCeCommand
	"IDbCommand.ExecuteReader",
	"IDbCommand.ExecuteScalar",

	"OdbcCommand.ExecuteReader",
	"OdbcCommand.ExecuteReaderAsync",
	"OdbcCommand.ExecuteScalar",
	"OdbcCommand.ExecuteScalarAsync",

	"OleDbCommand.ExecuteReader",
	"OleDbCommand.ExecuteScalar",
	"OleDbDataReader.ExecuteReader",

	"OracleCommand.ExecuteReader",
	"OracleCommand.ExecuteReaderAsync",
	"OracleCommand.ExecuteScalar",
	"OracleCommand.ExecuteScalarAsync",
	"OracleCommand.ExecuteOracleScalar",

	"SqlCommand.ExecuteReader",
	"SqlCommand.ExecuteReaderAsync",
	"SqlCommand.ExecuteScalar",
	"SqlCommand.ExecuteScalarAsync",
	"SqlCommand.ExecuteXmlReader",
	"SqlCommand.ExecuteXmlReaderAsync",

	"SqlCeCommand.ExecuteReader",
	"SqlCeCommand.ExecuteScalar",
	"SqlCeCommand.ExecuteResultSet",

	"EntityCommand.ExecuteScalar",
	"EntityCommand.ExecuteScalarAsync",
	"EntityCommand.ExecuteReader",
	"EntityCommand.ExecuteReaderAsync",

	//QSqlQuery
	"QSqlQuery.Exec",
	"QSqlQuery.ExecBatch",

	//DB2Command, UpdateCommand, InsertCommand, DeleteCommand and SelectCommand
	"Db2Command.ExecuteResultSet",
	"Db2Command.ExecuteXmlReader",
	"Db2Command.ExecuteReader",
	"Db2Command.ExecuteScalar",

	"UpdateCommand.ExecuteDbDataReader",
	"UpdateCommand.ExecuteReader",
	"UpdateCommand.ExecuteScalar",

	"InsertCommand.ExecuteDbDataReader",
	"InsertCommand.ExecuteReader",
	"InsertCommand.ExecuteScalar",

	"DeleteCommand.ExecuteDbDataReader",
	"DeleteCommand.ExecuteReader",
	"DeleteCommand.ExecuteScalar",

	"SelectCommand.ExecuteDbDataReader",
	"SelectCommand.ExecuteReader",
	"SelectCommand.ExecuteScalar",

	//SQlite
	"SqliteCommand.ExecuteReader",
	"SqliteCommand.ExecuteScalar",

	"SQLiteCommand.ExecuteReader",
	"SQLiteCommand.ExecuteReaderAsync",
	"SQLiteCommand.ExecuteScalar",
	"SQLiteCommand.ExecuteScalarAsync"}));

/* Commented because are already caught by FindByMemberAccesses in SqlCommand, due to the '*' by default in our solution.
 * If the '*' is removed from the solution, the next methods are necessary in the query 
 */ 
// result.Add(methods.FindByMemberAccesses(new string[]{
// 		"NpgsqlCommand.ExecuteReader",
// 		"NpgsqlCommand.ExecuteScalar",
// 		"NpgsqlCommand.ExecuteScalarAsync"}));
result.Add(methods.FindByMemberAccesses(new string[] {
	"NpgsqlCommand.ExecuteDbDataReader",
	"NpgsqlCommand.ExecuteDbDataReaderAsync"}));


// Implicit typed Executes
CxList executes = Find_DB_ImplicitTypedCommands()
	.GetMembersOfTarget().FindByShortNames(new List<string>{
		"ExecuteDbDataReader","ExecuteReader","ExecuteScalar"});

result.Add(executes);