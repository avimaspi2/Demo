CxList methods = Find_DB_In_DBO();
methods.Add(Find_DB_Out_DBO());

result.Add(methods.FindByName("*.execute"));
result.Add(methods.FindByName("*.execute_many"));
result.Add(methods.FindByName("*.insert"));
result.Add(methods.FindByName("*.update"));
result.Add(methods.FindByName("*.delete*"));
result.Add(methods.FindByName("*.drop*"));
result.Add(methods.FindByName("*.select*"));