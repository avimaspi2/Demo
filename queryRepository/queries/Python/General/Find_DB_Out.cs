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

result = Find_DB_Out_ADOdb(imports);
result.Add(Find_DB_Out_MySQL(imports));
result.Add(Find_DB_Out_MSSQL(imports)); 
result.Add(Find_DB_Out_OJDBC(imports)); 
result.Add(Find_DB_Out_ODBC(imports));
result.Add(Find_DB_Out_Oracle(imports)); 
result.Add(Find_DB_Out_DBO(imports));
result.Add(Find_DB_Out_Postgres(imports));
result.Add(Find_DB_Out_Django(imports));
result.Add(Find_DB_Out_Peewee(imports));
result.Add(Find_DB_Out_Postgres());
result.Add(Find_DB_Out_Mongo(imports));
result.Add(Find_DB_Out_SAP(imports));
result.Add(Find_DB_Out_SQLAlchemy());
result.Add(Find_DB_Out_SQLanywhere(imports));
result.Add(Find_DB_Out_SQLite(imports));
result.Add(Find_DB_Out_SqlObject(imports));
result.Add(Find_DB_Out_Sybase(imports));