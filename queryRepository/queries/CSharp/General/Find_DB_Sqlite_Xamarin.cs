/*  SQLiteConnection 
 */
CxList methods = Find_Methods();
CxList temp = methods.FindByMemberAccess("SqliteConnection.*");

result = temp.FindByMemberAccess("SqliteConnection.GetTableInfo ");
result.Add(temp.FindByMemberAccess("SqliteConnection.Execute"));
result.Add(temp.FindByMemberAccess("SqliteConnection.ExecuteScalar"));
result.Add(temp.FindByMemberAccess("SqliteConnection.Query"));
result.Add(temp.FindByMemberAccess("SqliteConnection.DeferredQuery"));
result.Add(temp.FindByMemberAccess("SqliteConnection.Get"));
result.Add(temp.FindByMemberAccess("SqliteConnection.Find"));
result.Add(temp.FindByMemberAccess("SqliteConnection.FindWithQuery"));
result.Add(temp.FindByMemberAccess("SqliteConnection.InsertAll "));
result.Add(temp.FindByMemberAccess("SqliteConnection.Insert"));
result.Add(temp.FindByMemberAccess("SqliteConnection.InsertOrReplace"));
result.Add(temp.FindByMemberAccess("SqliteConnection.Update"));
result.Add(temp.FindByMemberAccess("SqliteConnection.UpdateAll"));
result.Add(temp.FindByMemberAccess("SqliteConnection.Delete"));
result.Add(temp.FindByMemberAccess("SqliteConnection.DeleteAll"));


//SqliteCommand
result.Add(methods.FindByMemberAccess("SqliteCommand.ExecuteDeferredQuery"));
result.Add(methods.FindByMemberAccess("SqliteCommand.ExecuteQuery"));