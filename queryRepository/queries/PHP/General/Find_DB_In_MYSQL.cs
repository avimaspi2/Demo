CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList mysql = methods.FindByShortName("mysql*", false);

CxList directDbMethods = methods.FindByShortNames(new List<string> {"mysql_query", "mysql_db_query", "mysql_unbuffered_query", 
	"mysqli_multi_query", "mysqli_prepare","mysqli_query","mysqli_real_query","mysqli_stmt_execute",
	"mysqli_stmt_prepare","mysqli_bind_param", "mysqli_stmt_bind_param", "mysqli_master_query", "mysqli_send_query",
		"mysqli_slave_query"});

/*
CxList directDbMethods = 
	mysql.FindByShortName("mysql_query") + 
	mysql.FindByShortName("mysql_db_query") + 
	mysql.FindByShortName("mysql_unbuffered_query") +
	//methods.FindByShortName("multi_query") + 
	mysql.FindByShortName("mysqli_multi_query") +
	mysql.FindByShortName("mysqli_prepare") +
	mysql.FindByShortName("mysqli_query") +
	mysql.FindByShortName("mysqli_real_query") +
	mysql.FindByShortName("mysqli_stmt_execute") +
	mysql.FindByShortName("mysqli_stmt_prepare") +
	mysql.FindByShortName("mysqli_bind_param") +
	mysql.FindByShortName("mysqli_stmt_bind_param") + 
	mysql.FindByShortName("mysqli_master_query") +
	mysql.FindByShortName("mysqli_send_query") +
	mysql.FindByShortName("mysqli_slave_query");
*/

result.Add(directDbMethods);

CxList directDbMethods_Deprecated =
	mysql.FindByShortName("mysql_db_query");
result.Add(directDbMethods_Deprecated);

// 2 - Find query\execute function calling on object initialize with DB.
CxList pDbExecuteQuery = methods.FindByShortNames(new List<string> {"query", "real_query", "prepare", 
	"multi_query", "bind_param"}, false);
/*
	CxList pDbExecuteQuery = 
	methods.FindByShortName("query", false) + 
	methods.FindByShortName("real_query", false) + 
	methods.FindByShortName("prepare", false) + 
	//methods.FindByShortName("execute", false) +
	methods.FindByShortName("multi_query", false) +	
	methods.FindByShortName("bind_param", false);
*/
	
CxList pDbExecuteQueryMembers = pDbExecuteQuery.GetTargetOfMembers();

CxList pDbCreation = All.FindByType(typeof(ObjectCreateExpr));
//CxList objectCreateMySQL = pDbCreation.FindByShortName("mysql*", false);
// Object Oriented
pDbCreation = pDbCreation.FindByShortNames(new List<string> {"mysql", "mysqli", "mysqli_stmt"}, false);
/*
pDbCreation = 
	objectCreateMySQL.FindByShortName("mysql", false) + 
	objectCreateMySQL.FindByShortName("mysqli", false) +
	objectCreateMySQL.FindByShortName("mysqli_stmt", false) +
	*/
	// Procedural alias
pDbCreation.Add(mysql.FindByShortName("mysqli_connect"));
//mysql.FindByShortName("mysqli_connect");

CxList pDbCreationLeftSide = All.NewCxList();
CxList assignExpr = pDbCreation.GetAncOfType(typeof(AssignExpr));
foreach (CxList assign in assignExpr)
{
	AssignExpr g = assign.data.GetByIndex(0) as AssignExpr;
	pDbCreationLeftSide.Add(All.FindById(g.Left.NodeId));
}

CxList dbFunctionByInstantiation = pDbExecuteQueryMembers.DataInfluencedBy(pDbCreationLeftSide).GetMembersOfTarget();
/*
CxList bindedParams = All.FindByType(typeof(Param)).GetParameters(dbFunctionByInstatniation.FindByShortName("bind_param"));
dbFunctionByInstatniation -= dbFunctionByInstatniation.FindByShortName("bind_param");*/
result.Add(dbFunctionByInstantiation);// -dbFunctionByInstatniation.InfluencedBy(directDbMethods);
//result.Add(bindedParams);