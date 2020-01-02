CxList methods = Find_Methods();

CxList ociType = All.FindByType("OCI8");

CxList ociAssign = ociType.FindByType(typeof(TypeRef)).GetAncOfType(typeof(AssignExpr));
CxList connection = All.GetByAncs(ociAssign).FindByAssignmentSide(CxList.AssignmentSide.Left) + 
	ociType.FindByType(typeof(UnknownReference));
connection.Add(All.GetByAncs(connection.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left));
connection.Add(All.GetByAncs(connection));
CxList commands = All.FindAllReferences(connection) + 
	All.FindAllReferences(connection.GetTargetOfMembers()).GetMembersOfTarget() +
	All.FindByMemberAccess("OCI8.*");

commands.Add(commands.GetMembersOfTarget());
commands -= commands.FindByAssignmentSide(CxList.AssignmentSide.Left);
commands = commands.FindByType(typeof(MemberAccess)) + commands.FindByType(typeof(MethodInvokeExpr));

string[] dbCommandsList = new string[] {
	"all",
	"delete" ,
	"exec",
	"execute",
	"first",
	"one",
	"parse",
	"select",
	"select_all",
	"select_first",
	"select_hash_all",
	"select_hash_first"
	};

foreach (string s in dbCommandsList)
{
	result.Add(commands.FindByShortName(s));
}
	
result.Add(Add_Second_Order_DB(result, dbCommandsList));