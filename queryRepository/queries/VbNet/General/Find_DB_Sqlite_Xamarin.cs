CxList methods = Find_Methods();

result = methods.FindByMemberAccess("SQLiteConnection.GetTableInfo", false);
result.Add(methods.FindByMemberAccess("SQLiteConnection.Execute", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.ExecuteScalar", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.Query", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.DeferredQuery", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.Get", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.Find", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.FindWithQuery", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.InsertAll", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.Insert", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.InsertOrReplace", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.Update", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.UpdateAll", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.Delete", false));
result.Add(methods.FindByMemberAccess("SQLiteConnection.DeleteAll", false));


//SqliteCommand
result.Add(methods.FindByMemberAccess("SQLiteCommand.ExecuteDeferredQuery", false));
result.Add(methods.FindByMemberAccess("SQLiteCommand.ExecuteQuery", false));