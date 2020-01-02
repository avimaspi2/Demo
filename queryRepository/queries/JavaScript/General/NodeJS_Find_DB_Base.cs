CxList input = All.NewCxList();
CxList allINSQLmysql = NodeJS_Get_All_In_DB();
//--------------------------------------------------------------------------------------------------
CxList methInvSQLmysql = allINSQLmysql.FindByType(typeof (MethodInvokeExpr));
//only query() methods in files that include db-mysql or mysql DB 
CxList sqlQuerymysql = methInvSQLmysql.FindByMemberAccess("*.query");
sqlQuerymysql.Add(methInvSQLmysql.FindByMemberAccess("*.execute"));

input.Add(sqlQuerymysql);
//--------------------------------------------------------------------------------------------------
 
result = input;