CxList methods = Find_Methods();
CxList dbMethods = Find_DB_In();
dbMethods.Add(Find_DB_Out());

List<string> allMethod = new List<string> {
		"mysql_*",
		"mysqli*",
		"pg_*",
		"mssql_*",
		"oci*", 
		"dbx_*",
		"odbc_*",
		"dba_*"
		};

dbMethods.Add(methods.FindByShortNames(allMethod));

/* Filter out all methods declared within the code itself */

/* Find all methods declarations */
CxList method_decls = Find_MethodDecls();
/* Find referances to db methods declared within the code */
CxList method_ref = dbMethods.FindAllReferences(method_decls);

result = dbMethods - method_ref; // remove references to db methods which declared within the code