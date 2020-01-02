CxList parameters = All.FindByMemberAccess("*.Parameters");
CxList parametersUS = All.FindByMemberAccess("*.Parameters_*");
CxList command = All.FindByShortName("*Command");

CxList allParameters = parameters + parametersUS;
result = allParameters.FindByMemberAccess("SqlCommand.*") +
	allParameters.FindByMemberAccess("OracleCommand.*") +
	allParameters.FindByMemberAccess("OdbcCommand.*") +
	allParameters.FindByMemberAccess("OleDbCommand.*");

CxList parametersTarget = allParameters.GetTargetOfMembers();
parametersTarget = parametersTarget * command;
CxList maCommands = parametersTarget.FindByShortNames(new List<string>
	{"SelectCommand", "UpdateCommand", "InsertCommand", "DeleteCommand"});

//	parametersTarget.FindByShortName("SelectCommand") +
//	parametersTarget.FindByShortName("UpdateCommand") +
//	parametersTarget.FindByShortName("InsertCommand") +
//	parametersTarget.FindByShortName("DeleteCommand");
result.Add(maCommands.GetTargetOfMembers().GetMembersOfTarget().GetMembersOfTarget());

	
result.Add(All.FindByName("*_quote*") +
	All.FindByName("*getTableName*"));

CxList addParam = All.FindByMemberAccess("*.AddInParameter") +
	All.FindByMemberAccess("*.AddOutParameter") +
	All.FindByMemberAccess("*.AddParameter");


CxList database = addParam.FindByMemberAccess("*Database.*");
CxList db = database.GetTargetOfMembers();

db = db.FindByShortNames(new List<string>{"Database", "OracleDatabase", "SqlDatabase", "GenericDataBase"});

/*
db = db.FindByShortName("Database") +
	db.FindByShortName("OracleDatabase") +
	db.FindByShortName("SqlDatabase") +
	db.FindByShortName("GenericDataBase");
*/
result.Add(db.GetMembersOfTarget());
	
result.Add(result.GetAncOfType(typeof(MethodInvokeExpr)));