//////////////////////////////////////////////////////////////////////////////////
// Query: Dynamic_SQL_Queries 
// Purpose: Find SQL queries that are built of concatenation of strings
//

CxList db = Find_DB_In();
//find all fixed strings
CxList strings = Find_Strings();
strings -= strings.FindByName("*$*");

CxList methods = Find_Methods();
CxList DoubleQuotedStrings = methods.FindByShortName("$_DoubleQuotedString");
CxList strParams = All.GetParameters(DoubleQuotedStrings) - All.GetParameters(DoubleQuotedStrings, 0);
strParams = strParams.DataInfluencingOn(db).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

CxList binary = All.FindByType(typeof(BinaryExpr));
CxList concat = All.NewCxList();

//Filter out all binary expressions that are not Concat
foreach(CxList s in binary)
{
	BinaryExpr ae = s.data.GetByIndex(0) as BinaryExpr; 
	if(ae != null && ae.Operator.ToString().Equals("Concat"))
	{		
		concat.Add(s);
	}
}

// Treat AdditionAssign, e.g.
CxList assign = All.FindByType(typeof(AssignExpr));
CxList tmp = All.NewCxList();

foreach(CxList s in assign)
{
	AssignExpr ae = s.data.GetByIndex(0) as AssignExpr;
	if(ae != null && ae.Operator.ToString().Equals("ConcatAssign"))
	{	
		//remove .= "Fixed strings"
		tmp = strings.GetByAncs(s);
		if (tmp.Count == 0)
			concat.Add(s);
		else
			strParams -= tmp;
	}
}

concat.Add(strParams);

result = concat.InfluencingOn(db, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)
	.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow)
	.ReduceFlowByPragma();