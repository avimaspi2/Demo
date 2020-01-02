//This query checks whether a catch block is either empty or only has System.debug in it.
CxList catchStmt = All.FindByType(typeof(Catch));
CxList debug = All.FindByMemberAccess("System.debug",false);

CxList statements = All.FindByType(typeof(Statement));
foreach(CxList Catch in catchStmt)
{
	CxList underCatchStmt = statements.GetByAncs(Catch);
	if(underCatchStmt.Count == 0)
	{
		result.Add(Catch);
	}else
	{
		if(underCatchStmt.Count == 1)
		{
			if((debug.GetByAncs(Catch)).Count > 0)
			{
				result.Add(Catch);
			}
		}
	}              
}