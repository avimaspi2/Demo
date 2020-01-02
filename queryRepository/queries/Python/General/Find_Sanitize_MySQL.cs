CxList methods = Find_DB_In_MySQL();
methods.Add(Find_DB_Out_MySQL());

CxList allParams = Find_Param();

// http://dev.mysql.com/doc/connector-python/en/connector-python-api-mysqlcursor.html
CxList mysql_execute = methods.FindByName("*.execute");
CxList mysql_executemany = methods.FindByName("*.executemany");

//only the second parameter of execute/executemany is sanitized
result.Add(allParams.GetParameters(mysql_execute, 1));
result.Add(allParams.GetParameters(mysql_executemany, 1));