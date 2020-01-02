CxList embedded_sql = All.FindByName("CX_SQL_STATEMENT", false);

CxList sql_stat = All.NewCxList();
while (embedded_sql.Count > 0)
{
	embedded_sql = embedded_sql.GetMembersOfTarget();
	sql_stat.Add(embedded_sql);
}
	
result = All.GetParameters(sql_stat);