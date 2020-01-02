CxList methods = Find_Methods();

//SqlDataSource and AccessDataSource
result = methods.FindByMemberAccess("SqlDataSource.Update", false);
result.Add(methods.FindByMemberAccess("SqlDataSource.Insert", false));
result.Add(methods.FindByMemberAccess("SqlDataSource.Delete", false));
result.Add(methods.FindByMemberAccess("SqlDataSource.select", false));

result.Add(methods.FindByMemberAccess("AccessDataSource.Update", false));
result.Add(methods.FindByMemberAccess("AccessDataSource.Insert", false));
result.Add(methods.FindByMemberAccess("AccessDataSource.Delete", false));
result.Add(methods.FindByMemberAccess("AccessDataSource.Select", false));


//IDbCommand, OdbcCommand, OleDbCommand, OracleCommand, 
//EntityCommand, SqlCommand, SqlCeCommand  
result.Add(methods.FindByMemberAccess("IDbCommand.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("IDbCommand.ExecuteScalar", false));

result.Add(methods.FindByMemberAccess("OdbcCommand.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("OdbcCommand.ExecuteScalar", false));

result.Add(methods.FindByMemberAccess("OleDbCommand.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("OleDbCommand.ExecuteScalar", false));
result.Add(methods.FindByMemberAccess("OleDbDataReader.ExecuteReader", false));

result.Add(methods.FindByMemberAccess("OracleCommand.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("OracleCommand.ExecuteScalar", false));
result.Add(methods.FindByMemberAccess("OracleCommand.ExecuteOracleScalar", false));

result.Add(methods.FindByMemberAccess("SqlCommand.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("SqlCommand.ExecuteScalar", false));
result.Add(methods.FindByMemberAccess("SqlCommand.executexmlreader", false));

result.Add(methods.FindByMemberAccess("SqlCeCommand.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("SqlCeCommand.ExecuteScalar", false));
result.Add(methods.FindByMemberAccess("SqlCeCommand.executeresultset", false));

result.Add(methods.FindByMemberAccess("EntityCommand.ExecuteScalar", false));
result.Add(methods.FindByMemberAccess("EntityCommand.ExecuteReader", false));

//QSqlQuery
result.Add(methods.FindByMemberAccess("QSqlQuery.exec", false));
result.Add(methods.FindByMemberAccess("QSqlQuery.execBatch", false));


//DB2Command, UpdateCommand, InsertCommand, DeleteCommand and SelectCommand
result.Add(methods.FindByMemberAccess("DB2Command.ExecuteResultSet", false));
result.Add(methods.FindByMemberAccess("DB2Command.ExecutXxmlReader", false));
result.Add(methods.FindByMemberAccess("DB2Command.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("DB2Command.ExecuteScalar", false));

result.Add(methods.FindByMemberAccess("UpdateCommand.ExecuteDbDataReader", false));
result.Add(methods.FindByMemberAccess("UpdateCommand.ExecuteReader", false)); 
result.Add(methods.FindByMemberAccess("UpdateCommand.ExecuteScalar", false)); 

result.Add(methods.FindByMemberAccess("InsertCommand.ExecuteDbDataReader", false));
result.Add(methods.FindByMemberAccess("InsertCommand.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("InsertCommand.ExecuteScalar", false));

result.Add(methods.FindByMemberAccess("DeleteCommand.ExecuteDbDataReader", false));
result.Add(methods.FindByMemberAccess("DeleteCommand.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("DeleteCommand.ExecuteScalar", false));

result.Add(methods.FindByMemberAccess("SelectCommand.ExecuteDbDataReader", false));
result.Add(methods.FindByMemberAccess("SelectCommand.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("SelectCommand.ExecuteScalar", false));

//SQlite
result.Add(methods.FindByMemberAccess("SqliteCommand.ExecuteReader", false));
result.Add(methods.FindByMemberAccess("SqliteCommand.ExecuteScalar", false));