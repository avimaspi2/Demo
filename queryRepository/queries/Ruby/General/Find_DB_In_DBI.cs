CxList methods = Find_Methods();

CxList dbDef = methods.FindByMemberAccess("DBI.connect");

dbDef = dbDef.GetTargetOfMembers();
CxList dbAll = All * All.DataInfluencedBy(dbDef);

string[] dbCommands = new string[] {
	"select_one*",
	"select_all",
	"do",
	"prepare"	
	};

foreach (string s in dbCommands)
{
	result.Add(dbAll.FindByShortName(s));
}
	
result.Add(Add_Second_Order_DB(result, dbCommands));