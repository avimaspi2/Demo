CxList dbAll = Find_MongoDB_Def();
dbAll.Add(All.GetByAncs(dbAll.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left));
dbAll.Add(All.FindAllReferences(dbAll));
dbAll.Add(All.FindByType(typeof(UnknownReference)).InfluencedByAndNotSanitized(dbAll, All.FindByType(typeof(AssignExpr)) + All.FindByType(typeof(BinaryExpr))));

CxList dbBasicCommands = dbAll.GetMembersOfTarget();
CxList dbCollections = 
	dbBasicCommands.FindByShortName("collection") + 
	dbAll.FindByType(typeof(IndexerRef));

dbCollections.Add(All.FindByName("*collection.*"));
dbCollections = All.GetByAncs(dbCollections.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
dbCollections.Add(All.FindAllReferences(dbCollections));

CxList dbCommands = dbCollections.GetMembersOfTarget();
dbCommands.Add(All.FindByName("*collection.*"));
dbCommands.Add(dbBasicCommands.GetMembersOfTarget());
dbCommands.Add(dbCommands.GetMembersOfTarget());
dbCommands.Add(dbCommands.GetMembersOfTarget());
dbCommands.Add(All.FindByShortName("order_by").GetMembersOfTarget());

string[] dbCommandsList = new string[] {
	"insert", // output only...
	"update",
	"save",
	"find_one",
	"find",
	"group",
	"runCommand",
	"remove"
	};

foreach (string s in dbCommandsList)
{
	result.Add(dbCommands.FindByShortName(s));
}
	
result.Add(Add_Second_Order_DB(result, dbCommandsList));