//SQLite
//https://docs.python.org/2/library/sqlite3.html
//sanitizing methods
// // execute(*)
// // executemany(...)
//* the number of arguments vary. 
//If the number of arguments is 1, then SQL Injection may be exploited

CxList methods = Find_DB_In_SQLite();
methods.Add(Find_DB_Out_SQLite());

CxList allParams = Find_Param();

CxList execute_mthds = methods.FindByName("*.execute");
CxList executemany_mthds = methods.FindByName("*.executemany");

//only the second parameter of query is sanitized
result.Add(allParams.GetParameters(execute_mthds, 1));
result.Add(allParams.GetParameters(executemany_mthds, 1));