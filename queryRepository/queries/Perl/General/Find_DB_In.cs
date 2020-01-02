CxList methods = Find_Methods();

CxList dbDef = methods.FindByMemberAccess("DBI", "connect") + methods.FindByMemberAccess("DBI", "connect_cached");

CxList dbAll = methods.DataInfluencedBy(dbDef);

// dbi
string[] dbCommands = new string[] {
	"*select*",
	"do",
	"prepare*",
	"execute*",
	};

CxList dbPrepare = dbAll.FindByShortName("prepare*");
CxList badDbPrepare = dbPrepare - dbPrepare.DataInfluencingOn(dbAll.FindByShortName("execute") + dbAll.FindByShortName("execute_array"));
dbAll -= badDbPrepare;

foreach (string s in dbCommands)
{
	result.Add(All.GetParameters(dbAll.FindByShortName(s), 0));
}

// oracle
result.Add(All.GetParameters(methods.FindByShortName("ora_open"), 1));
result.Add(All.GetParameters(methods.FindByShortName("ora_do"), 1));

// mysql
result.Add(methods.FindByShortName("query"));

string[] allDBCommands = new string[] {
	"*select*",
	"do",
	"prepare*",
	"fetchrow_*",
	"ora_open",
	"ora_do",
	"ora_fetch"
	};

//result.Add(Add_Second_Order_DB(result, allDBCommands));