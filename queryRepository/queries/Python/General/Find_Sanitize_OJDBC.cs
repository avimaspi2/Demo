CxList methods = Find_DB_In_OJDBC();
methods.Add(Find_DB_Out_OJDBC());

CxList allParams = Find_Param();

// https://pypi.python.org/pypi/JayDeBeApi/
CxList ojdbc_execute = methods.FindByName("*.execute");
CxList ojdbc_executemany = methods.FindByName("*.executemany");

//only the second parameter of execute/executemany is sanitized
result.Add(allParams.GetParameters(ojdbc_execute, 1));
result.Add(allParams.GetParameters(ojdbc_executemany, 1));