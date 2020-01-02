CxList methods = Find_Methods();

CxList dbDef = methods.FindByMemberAccess("Mysql.real_connect") +
	methods.FindByMemberAccess("Mysql.connect") +
	methods.FindByMemberAccess("Mysql.new");

CxList dbInit = All.FindByMemberAccess("Mysql.init");
dbInit = methods.DataInfluencedBy(dbInit);
dbInit.Add(All.FindAllReferences(dbInit.GetTargetOfMembers()).GetMembersOfTarget());

dbDef.Add(dbInit.FindByShortName("real_connect") + 
	dbInit.FindByShortName("connect") + 
	dbInit.FindByShortName("new"));
dbDef = dbDef.GetTargetOfMembers();

CxList dbAll = All * All.DataInfluencedBy(dbDef);

string[] dbCommandsList = new string[] {
	"query",
	"real_query",
	"prepare",
	"execute"
	};

foreach (string s in dbCommandsList)
{
	result.Add(dbAll.FindByShortName(s));
}
	
result.Add(Add_Second_Order_DB(result, dbCommandsList));

CxList dbDefAssign = dbDef.GetAncOfType(typeof(AssignExpr));
dbDefAssign = All.GetByAncs(dbDefAssign).FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList query = All.FindAllReferences(dbDefAssign).GetMembersOfTarget().FindByShortName("query");
result.Add(query);

// 1 - Explicite DB function names
CxList directDbMethods = 
	methods.FindByShortName("mysql_query") + 
	methods.FindByShortName("mysql_db_query") + 
	methods.FindByShortName("mysql_unbuffered_query") +
	methods.FindByShortName("multi_query") + 
	methods.FindByShortName("mysqli_multi_query") +
	methods.FindByShortName("mysqli_prepare") +
	methods.FindByShortName("mysqli_query") +
	methods.FindByShortName("mysqli_real_query") +
	methods.FindByShortName("mysqli_stmt_execute") +
	methods.FindByShortName("mysqli_stmt_prepare") +
	methods.FindByShortName("mysqli_bind_param") +
	methods.FindByShortName("mysqli_stmt_bind_param") +
	methods.FindByShortName("mysqli_master_query") +
	methods.FindByShortName("mysqli_send_query") +
	methods.FindByShortName("mysqli_slave_query");

result.Add(directDbMethods);

CxList directDbMethods_Deprecated =
	methods.FindByShortName("mysql_db_query");
result.Add(directDbMethods_Deprecated);

// 2 - Find query\execute function calling on object initialize with DB.
CxList pDbExecuteQuery = 
	methods.FindByShortName("query", false) + 
	methods.FindByShortName("real_query", false) + 
	methods.FindByShortName("prepare", false) + 
	methods.FindByShortName("execute", false);
CxList pDbExecuteQueryMembers = pDbExecuteQuery.GetTargetOfMembers();

CxList pDbCreation = All.FindByType(typeof(ObjectCreateExpr));

pDbCreation = 
	pDbCreation.FindByShortName("mysql", false) + 
	pDbCreation.FindByShortName("mysqli", false) +
	pDbCreation.FindByShortName("mysqli_stmt", false);


CxList pDbCreationLeftSide = All.NewCxList();
foreach (CxList assign in pDbCreation.GetAncOfType(typeof(AssignExpr)))
{
	AssignExpr g = assign.data.GetByIndex(0) as AssignExpr;
	pDbCreationLeftSide.Add(All.FindById(g.Left.NodeId));
}

CxList dbFunctionByInstatniation = pDbExecuteQueryMembers.InfluencedBy(pDbCreationLeftSide).GetMembersOfTarget();

result.Add(dbFunctionByInstatniation);// -dbFunctionByInstatniation.InfluencedBy(directDbMethods);