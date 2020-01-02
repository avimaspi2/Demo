CxList methods = Find_Methods();
CxList dbMethods = Find_DB_In();
dbMethods.Add(Find_DB_Out());

dbMethods.Add(
	methods.FindByShortName("mysql_*") +
	methods.FindByShortName("mysqli*") +
	methods.FindByShortName("pg_*") +
	methods.FindByShortName("mssql_*") +
	methods.FindByShortName("oci*") + 
	methods.FindByShortName("dbx_*") +
	methods.FindByShortName("odbc_*") +
	methods.FindByShortName("dba_*"));

/* Filter out all methods declared within the code itself */

/* Find all methods declarations */
CxList method_decls = All.FindByType(typeof(MethodDecl));
/* Find referances to db methods declared within the code */
CxList method_ref = dbMethods.FindAllReferences(method_decls);

result = dbMethods - method_ref; // remove references to db methods which declared within the code