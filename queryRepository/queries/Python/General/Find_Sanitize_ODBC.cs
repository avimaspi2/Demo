CxList methods = Find_DB_In_ODBC();
methods.Add(Find_DB_Out_ODBC());

CxList allParams = Find_Param();

// https://code.google.com/p/pyodbc/wiki/GettingStarted
// https://code.google.com/p/pypyodbc/wiki/PyPyODBC_Example_Tutorial
// http://ceodbc.sourceforge.net/html/cursor.html
CxList odbc_execute = methods.FindByName("*.execute");

// http://ceodbc.sourceforge.net/html/cursor.html (executemany exists in ceodbc)
CxList odbc_executemany = methods.FindByName("*.executemany");

//only the second parameter of execute/executemany is sanitized
result.Add(allParams.GetParameters(odbc_execute, 1));
result.Add(allParams.GetParameters(odbc_executemany, 1));