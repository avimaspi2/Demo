CxList dbMethods = Find_DB_Out_ADODB();

dbMethods.Add(Find_DB_Out_DB2());
dbMethods.Add(Find_DB_Out_DBA());
dbMethods.Add(Find_DB_Out_DBX());
dbMethods.Add(Find_DB_Out_FrontBase());
dbMethods.Add(Find_DB_Out_Ifx());
dbMethods.Add(Find_DB_Out_Ingres());
dbMethods.Add(Find_DB_Out_InterBase());
dbMethods.Add(Find_DB_Out_MDB2());
dbMethods.Add(Find_DB_Out_MSQL());
dbMethods.Add(Find_DB_Out_MSSQL());
dbMethods.Add(Find_DB_Out_MYSQL());
dbMethods.Add(Find_DB_Out_ODBC());
dbMethods.Add(Find_DB_Out_Ora());
dbMethods.Add(Find_DB_Out_ORACLE());
dbMethods.Add(Find_DB_Out_Ovrimos());
dbMethods.Add(Find_DB_Out_PDO());
dbMethods.Add(Find_DB_Out_PG());
dbMethods.Add(Find_DB_Out_SQLite());
dbMethods.Add(Find_DB_Out_Sybase());
dbMethods.Add(Find_DB_Out_WordPress());
dbMethods.Add(Find_DB_Out_Creole());

/* Filter out all methods declared within the code itself */

/* Find all methods declarations */
CxList method_decls = All.FindByType(typeof(MethodDecl));
/* Find referances to db methods declared within the code */
CxList method_ref = dbMethods.FindAllReferences(method_decls);

result = dbMethods - method_ref; // remove references to db methods which declared within the code
result.Add(Find_Zend_DB_Out() + Find_Kohana_DB_Out()+Find_Cake_DB_Out());

result.Add(All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("Resultset"));

//memcache: add get methods influenced by the query
result.Add(Find_memcache_Inputs(result));
result.Add(Find_Doctrine_DB_Out());