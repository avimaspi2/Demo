CxList imports = All.NewCxList();

if (param.Length == 1)
{
	try
	{
		imports = param[0] as CxList;
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
} else {
	imports = Find_Imports();
}
CxList DbConn = Find_DB_Conn_DataFile(imports);
CxList allDataInfluencedByConn = All.DataInfluencedBy(DbConn);
CxList target = allDataInfluencedByConn.GetMembersOfTarget();

//Explicit insert functions
result.Add(target.FindByShortName("insert*"));