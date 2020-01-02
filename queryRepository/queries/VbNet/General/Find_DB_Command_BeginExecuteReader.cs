CxList methods = Find_Methods();

result = methods.FindByMemberAccess("SqlCommand.BeginExecuteReader", false);
result.Add(methods.FindByMemberAccess("SqlCommand.BeginExecuteXmlReader", false));
result.Add(methods.FindByMemberAccess("SqlCeCommand.ExecuteXmlReader", false));
result.Add(methods.FindByMemberAccess("SqlCeCommand.BeginExecuteReader", false));
result.Add(methods.FindByMemberAccess("SqlCeCommand.BeginExecuteXmlReader", false));
result.Add(methods.FindByMemberAccess("SqlCeCommand.BeginExecuteNonQuery", false));
result.Add(methods.FindByMemberAccess("SqlCommand.BeginExecuteNonQuery", false));