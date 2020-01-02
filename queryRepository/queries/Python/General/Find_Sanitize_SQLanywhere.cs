//SQLanywhere 
//http://dcx.sybase.com/1101/en/dbprogramming_en11/python-writing.html
//Sanitizing methods
//// execute(*)
//// executemany(...)
//* the number of arguments vary. 
//If the number of arguments is 1, then SQL Injection may be exploited

CxList methods = Find_DB_In_SQLanywhere();
methods.Add(Find_DB_Out_SQLanywhere());

CxList allParams = Find_Param();

CxList execute_mthds = methods.FindByName("*.execute");
CxList executemany_mthds = methods.FindByName("*.executemany");

//only the second parameter of query is sanitized
result.Add(allParams.GetParameters(execute_mthds, 1));
result.Add(allParams.GetParameters(executemany_mthds, 1));