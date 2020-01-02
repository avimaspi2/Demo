CxList methods = Find_Methods();
CxList dbMethods = Find_DB_In();
dbMethods.Add(Find_DB_Out());

CxList addMethods = methods.FindByShortNames(new List<string> 
	{"mysql_*", "mysqli*", "pg_*","mssql_*","oci*","dbx_*", "odbc_*", "dba_*"});

dbMethods.Add(addMethods);

/* Filter out all methods declared within the code itself */

/* Find all methods declarations */
CxList method_decls = All.FindByType(typeof(MethodDecl));
/* Remove methods declared in plugins */
method_decls -= method_decls.FindByFileName("*\\Plugins\\Php\\*");
/* Find referances to db methods declared within the code */
CxList method_ref = dbMethods.FindAllReferences(method_decls);

result = dbMethods - method_ref; // remove references to db methods which declared within the code