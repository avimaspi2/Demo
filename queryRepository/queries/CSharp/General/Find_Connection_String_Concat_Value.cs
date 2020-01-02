/// <summary>
/// This query was built to return the connection strings with concat values
/// </summary>

CxList strings = Find_Strings();
CxList connections = Find_Connection_String();
CxList potentialConnString = strings.FindByShortName("*=*");

CxList connectionStringAssign = connections.GetMembersOfTarget().FindByShortName("ConnectionString").GetAssigner();


CxList binaryExpr = potentialConnString.GetFathers().FindByType(typeof(BinaryExpr));

binaryExpr = binaryExpr.DataInfluencingOn(connectionStringAssign);

CxList binaryExprAdd = All.NewCxList();


//Filters the BinaryExpr in order to return only BinaryExpr with the BinaryOpertor Add
foreach(CxList binary in binaryExpr)
{
	BinaryExpr node = binary.TryGetCSharpGraph<BinaryExpr>();
	
	if (node.Operator == BinaryOperator.Add)
	{
		binaryExprAdd.Add(binary);
	}
}

result.Add(binaryExprAdd - binaryExprAdd.FindByFathers(binaryExprAdd));