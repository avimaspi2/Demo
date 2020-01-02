CxList strings = Find_Strings();
CxList binaryExpressions = All.FindByType(typeof(BinaryExpr));
CxList rightSideOfRegexOperatorStrings = All.NewCxList();
foreach (CxList bExpr in binaryExpressions)
{
	try
	{
		CSharpGraph graph = bExpr.data.GetByIndex(0) as CSharpGraph;
		if(graph != null)
		{
			BinaryExpr binaryExpr = graph as BinaryExpr;
			if(binaryExpr != null)
			{
				if(binaryExpr.Operator == BinaryOperator.RegexFind && binaryExpr.Right != null) 
				{
					rightSideOfRegexOperatorStrings.Add(All.FindById(binaryExpr.Right.NodeId));
				}
			}
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList methods = Find_Methods();

result = rightSideOfRegexOperatorStrings + 
	//strings.FindByShortName("/*").FindByShortName("*/") +
	strings.FindByShortName("s/*").FindByShortName("*/") +
	strings.FindByShortName("m/*").FindByShortName("*/") +
	strings.FindByShortName("tr/*").FindByShortName("*/") +
	strings.FindByShortName("/*").FindByShortName("*/g") +
	strings.FindByShortName("s/*").FindByShortName("*/g") +
	strings.FindByShortName("m/*").FindByShortName("*/g") +
	strings.FindByShortName("tr/*").FindByShortName("*/g");

CxList relevantMethods = 
	methods.FindByShortName("m") +
	methods.FindByShortName("s") +
	methods.FindByShortName("tr") +
	methods.FindByShortName("split");
result.Add(strings.GetParameters(relevantMethods, 0));

result -= result.FindByShortName("/");