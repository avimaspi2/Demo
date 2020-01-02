CxList methods = Find_Methods();

CxList firstParameter = methods.FindByMemberAccess("QSqlQuery.addBindValue");
firstParameter.Add(methods.FindByShortName("SQLPutData"));
CxList secondParameter = methods.FindByMemberAccess("QSqlQuery.bindValue");
CxList eighthParameter = methods.FindByShortName("SQLBindParameter");

CxList escapeString = methods.FindByShortName("mysql_real_escape_string");
escapeString = All.GetParameters(escapeString, 1);

escapeString = All.FindAllReferences(escapeString).InfluencedBy(escapeString);

result = All.FindByMemberAccess("mysqlpp.escape");
result.Add(Find_Integers());
result.Add(All.GetParameters(firstParameter, 0));
result.Add(All.GetParameters(secondParameter, 1));
result.Add(All.GetParameters(eighthParameter, 7));
result.Add(escapeString);

result.Add(Find_DB_PostgreSQL_Sanitizers());