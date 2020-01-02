CxList imports = All.NewCxList();

if (param.Length == 1)
{
	try
	{
		imports = param[0] as CxList;
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
} else {
	imports = Find_Imports();
}

result = Find_DB_In_ADOdb(imports);
result.Add(Find_DB_In_MySQL(imports)); 
result.Add(Find_DB_In_MSSQL(imports));
result.Add(Find_DB_In_OJDBC(imports));
result.Add(Find_DB_In_ODBC(imports));
result.Add(Find_DB_In_Oracle(imports));
result.Add(Find_DB_In_DBO(imports)); 
result.Add(Find_DB_In_Postgres());
result.Add(Find_DB_In_Django(imports));
result.Add(Find_DB_In_Peewee(imports));
result.Add(Find_DB_In_Postgres(imports));
result.Add(Find_DB_In_Mongo(imports));
result.Add(Find_DB_In_SAP(imports));
result.Add(Find_DB_In_SQLAlchemy());
result.Add(Find_DB_In_SQLanywhere(imports));
result.Add(Find_DB_In_SQLite(imports));
result.Add(Find_DB_In_SqlObject(imports));
result.Add(Find_DB_In_DataFile(imports));
result.Add(Find_DB_In_Sybase(imports));