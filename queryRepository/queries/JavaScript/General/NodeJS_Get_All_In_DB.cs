//require of require('db-mysql') or mysql or express
CxList requireOfDBmysql = NodeJS_Find_DB_Driver();

////////////////////////////////////////////////////////////////////////////////////////////////////
/*
	Get file names of all files that required db-mysql or mysql DB and all query methods for those DB
*/
CxList allINSQLmysql = All.NewCxList();
List<String> allFilesWithDBmysql = new List<String>();
foreach(CxList reqDB in requireOfDBmysql)
{
	try
	{
		CSharpGraph reqDBGR = reqDB.GetFirstGraph();
		String rDBName = reqDBGR.LinePragma.FileName;
		if (!allFilesWithDBmysql.Contains(rDBName))
		{
			allFilesWithDBmysql.Add(rDBName);
			allINSQLmysql.Add(All.FindByFileName(rDBName));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}
result = allINSQLmysql;