CxList methods = Find_DB_In_MSSQL();
methods.Add(Find_DB_Out_MSSQL());

CxList allParams = Find_Param();

// http://pymssql.sourceforge.net/ref_pymssql.php 
CxList execute = methods.FindByName("*.execute");
CxList executemany = methods.FindByName("*.executemany");

//only the second parameter of execute/executemany is sanitized
result.Add(allParams.GetParameters(execute, 1));
result.Add(allParams.GetParameters(executemany, 1));