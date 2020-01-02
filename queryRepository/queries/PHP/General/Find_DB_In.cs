CxList dbMethods = Find_DB_In_ADODB();
dbMethods.Add(Find_DB_In_DB2());
dbMethods.Add(Find_DB_In_DBA());
dbMethods.Add(Find_DB_In_DBX());
dbMethods.Add(Find_DB_In_FrontBase());
dbMethods.Add(Find_DB_In_Ifx());
dbMethods.Add(Find_DB_In_Ingres());
dbMethods.Add(Find_DB_In_InterBase());
dbMethods.Add(Find_DB_In_MDB2());
dbMethods.Add(Find_DB_In_MYSQL());
dbMethods.Add(Find_DB_In_MSQL());
dbMethods.Add(Find_DB_In_MSSQL());
dbMethods.Add(Find_DB_In_ODBC());
dbMethods.Add(Find_DB_In_Ora());
dbMethods.Add(Find_DB_In_ORACLE());
dbMethods.Add(Find_DB_In_Ovrimos());
dbMethods.Add(Find_DB_In_PDO());	
dbMethods.Add(Find_DB_In_PG());
dbMethods.Add(Find_DB_In_SQLite());
dbMethods.Add(Find_DB_In_Sybase());
dbMethods.Add(Find_DB_In_WordPress());
dbMethods.Add(Find_DB_In_Yii());

CxList methods = Find_Methods();
CxList directDbMethods = 
	methods.FindByShortName("PMA_DBI_query");
dbMethods.Add(directDbMethods);

CxList PhalconOut = methods.FindByShortName("getReadConnection");
dbMethods.Add(PhalconOut.GetMembersOfTarget().FindByShortName("query"));

// 3 - Find queries and execute method invoke that are members of target named db
CxList general = methods.FindByShortName("*query*", false) + methods.FindByShortName("*exec*", false);
general = general.GetTargetOfMembers();
CxList generalCallToDb = general.FindByShortNames(new List<String>(){ "*db*", "*database*", "*data_base*" }, false).GetMembersOfTarget();

dbMethods.Add(generalCallToDb);

/* Filter out all methods declared within the code itself */

/* Find all methods declarations */
CxList method_decls = All.FindByType(typeof(MethodDecl));
/* Find referances to db methods declared within the code */
CxList method_ref = dbMethods.FindAllReferences(method_decls);

//Add the first parameter of db_query function
CxList dbQueryFunction = methods.FindByShortName("db_query");
dbQueryFunction.Add(All.FindAllReferences(dbQueryFunction)); 
CxList dbQueryparam = All.GetParameters(dbQueryFunction, 0);

result = dbMethods - method_ref; // remove references to db methods which declared within the code
result.Add(Find_Zend_DB_In());
result.Add(Find_Kohana_DB_In());
result.Add(Find_Cake_DB_In());
result.Add(Find_Doctrine_DB_In());
result.Add(Find_DB_In_Creole());
result.Add(dbQueryparam);