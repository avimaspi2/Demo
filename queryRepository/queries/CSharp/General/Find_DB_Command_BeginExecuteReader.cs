/* BeginExecuteReader(), BeginExecuteXmlReader() and
 * BeginExecuteNonQuery() for SqlCommand and SqlCeCommand
 */

CxList methods = Find_Methods();

result = methods.FindByMemberAccess("SqlCommand.BeginExecuteReader");
result.Add(methods.FindByMemberAccess("SqlCommand.BeginExecuteXmlReader"));
result.Add(methods.FindByMemberAccess("SqlCeCommand.ExecuteXmlReader"));
result.Add(methods.FindByMemberAccess("SqlCeCommand.BeginExecuteReader"));
result.Add(methods.FindByMemberAccess("SqlCeCommand.BeginExecuteXmlReader"));
result.Add(methods.FindByMemberAccess("SqlCeCommand.BeginExecuteNonQuery"));
result.Add(methods.FindByMemberAccess("SqlCommand.BeginExecuteNonQuery"));