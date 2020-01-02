CxList methods = Find_DB_In_Sybase();
methods.Add(Find_DB_Out_Sybase());

CxList allParams = Find_Param();

// http://python-sybase.sourceforge.net/sybase/node10.html
CxList sybase_execute = methods.FindByName("*.execute");
CxList sybase_executemany = methods.FindByName("*.executemany");

//only the second parameter of query is sanitized
result.Add(allParams.GetParameters(sybase_execute, 1));
result.Add(allParams.GetParameters(sybase_executemany, 1));