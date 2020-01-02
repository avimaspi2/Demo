// Find_DB_In_MDB2
CxList methods = Find_Methods();

CxList connections = methods.FindByMemberAccess("MDB2.factory") +
	methods.FindByMemberAccess("MDB2.connect") +
	methods.FindByMemberAccess("MDB2.singleton");

CxList querys = methods.FindByShortNames(new List<string> {"exec", "subSelect", "query", "queryAll", "queryCol",
	"queryOne","queryRow","standaloneQuery","_doQuery","replace", "prepare"});
/*	
	methods.FindByShortName("exec") + 
	methods.FindByShortName("subSelect") + 
	methods.FindByShortName("query") +
	methods.FindByShortName("queryAll") +
	methods.FindByShortName("queryCol") +
	methods.FindByShortName("queryOne") +
	methods.FindByShortName("queryRow") +
	methods.FindByShortName("standaloneQuery") +
	methods.FindByShortName("_doQuery") +
	methods.FindByShortName("replace") +
	methods.FindByShortName("prepare");
*/

CxList dbExtended = methods.FindByShortNames(new List<string> {"getAll", "getAssoc", "getBeforeID", "getCol", "getOne",
	"getRow","executeMultiple","execParam"});
/*	
	methods.FindByShortName("getAll") +
	methods.FindByShortName("getAssoc") +
	methods.FindByShortName("getBeforeID") +
	methods.FindByShortName("getCol") +
	methods.FindByShortName("getOne") +
	methods.FindByShortName("getRow") +
	methods.FindByShortName("executeMultiple") +
	methods.FindByShortName("execParam");
*/
result = All.GetParameters((dbExtended + querys).DataInfluencedBy(connections));