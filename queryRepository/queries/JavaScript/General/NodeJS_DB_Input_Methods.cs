CxList input = All.NewCxList();

CxList requireOfDBNano = Find_Require("nano");

List<String> allFilesWithDBNano = new List<String>();
CxList allINSQLNano = All.NewCxList();
foreach(CxList reqDB in requireOfDBNano)
{
	try
	{
		CSharpGraph reqDBGR = reqDB.GetFirstGraph();
		String rDBName = reqDBGR.LinePragma.FileName;
		if (!allFilesWithDBNano.Contains(rDBName))
		{
			allFilesWithDBNano.Add(rDBName);
			allINSQLNano.Add(All.FindByFileName(rDBName));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList methInvSQLNano = allINSQLNano.FindByType(typeof (MethodInvokeExpr));

CxList sqlQueryNano = methInvSQLNano.GetMembersWithTargets().FindByShortNames(
	new List<string>{
		"atomic",
		"bulk",
		"copy",
		"destroy",
		"insert"});

input.Add(sqlQueryNano);

////////////////////////////////////////////////////////////////////////////////////////////////////
// Add input methods for node-couchdb DB

CxList requireOfDBCouchdb = Find_Require("node-couchdb");

List<String> allFilesWithDBcouchdb = new List<String>();
CxList allINSQLcouchdb = All.NewCxList();
foreach(CxList reqDB in requireOfDBCouchdb)
{
	try
	{
		CSharpGraph reqDBGR = reqDB.GetFirstGraph();
		String rDBName = reqDBGR.LinePragma.FileName;
		if (!allFilesWithDBcouchdb.Contains(rDBName))
		{
			allFilesWithDBcouchdb.Add(rDBName);
			allINSQLcouchdb.Add(All.FindByFileName(rDBName));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList methInvSQLcouchdb = allINSQLcouchdb.FindByType(typeof (MethodInvokeExpr));

CxList sqlQueryCouchdb = methInvSQLcouchdb.GetMembersWithTargets().FindByShortNames(
	new List<string>{
		"del",
		"insert",
		"update",
		"insertAttachment",
		"updateFunction"});


input.Add(sqlQueryCouchdb);
input.Add(NodeJS_MongoDB_Input_Methods());

result = input;