// Finds Implicit Commands on Databases where 
// FindByMemberAccess(*Command.execute*) does not find results, e.g.
// var conn = new SqlConnection(_connectionString);
// var cmd = conn.CreateCommand();
// cmd.Execute()
CxList ur = Find_Unknown_References();
CxList connections = ur.FindByTypes(new string[]{
	"IDbConnection","OdbcConnection","OleDbConnection",
	"OracleConnection","SqlConnection","SqlCeConnection",
	"EntityConnection","Db2Connection","SQLiteConnection"
	});

CxList commandMembers = ur.FindAllReferences(connections)
	.GetMembersOfTarget();
CxList commandDeclsOrAssigns = commandMembers
	.FindByShortNames(new List<string>{
		"CreateCommand", "CreateDbCommand"}).GetFathers();

// Collect only implicit typed variables, e.g. 
// var cmd = connection.CreateCommand();
CxList commandOrigins = commandDeclsOrAssigns
	.FindByType(typeof(Declarator)).FindByTypes(new string[]{"object", "var"});

commandOrigins.Add(ur.FindByFathers(commandDeclsOrAssigns
	.FindByType(typeof(AssignExpr)))
	.FindByAssignmentSide(CxList.AssignmentSide.Left));

result = ur.FindAllReferences(commandOrigins);