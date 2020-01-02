result = Find_Integers();
result.Add(Find_Dead_Code_Contents()); 
result.Add(All.FindByShortName("checkUrl"));
result.Add(All.FindByShortName("testUrl"));
result.Add(All.FindByMemberAccess("*Request.getContextPath"));

// Now we'll look for string.concat that attatchs a ? before the input (since this isn't an Open_Redirect)
// This query isn't perfict. It misses a lot of cases, but it's the best that I can do.

// Find the + that serve as string.concat
CxList plus = base.Find_BinaryExpr();
plus = plus.GetByBinaryOperator(BinaryOperator.Add);

CxList strings = Find_Strings(); // We only want the + that opporate on strings
strings.Add(All.FindByType("String"));

plus = strings.FindByFathers(plus);
plus = plus.GetFathers();
plus = plus.FindByType(typeof(BinaryExpr));

// Find all strings with ? in them
CxList stringsWithQuestionMarks = strings.FindByShortName("*?*");

// Find all stinrgs concat after a question mark
CxList sanitizers = All.NewCxList();
foreach (CxList p in plus) // for strings added with +
{
	try
	{
		BinaryExpr binaryExpr = p.TryGetCSharpGraph<BinaryExpr>();
		if (binaryExpr != null && binaryExpr.Right != null && binaryExpr.Left != null)
		{
			Expression right = binaryExpr.Right;
			CxList cxRight = All.FindById(right.NodeId);
			Expression left = binaryExpr.Left;
			while (left is BinaryExpr) // this is for cases like string1+string2+string3. There're a bit hard to deal with - and this is what the while loop dose
			{
				if (left != null && ((BinaryExpr) left).Right != null)
				{
					Expression leftRight = ((BinaryExpr) left).Right;
					CxList cxLeftRight = All.FindById(leftRight.NodeId);
					if ((stringsWithQuestionMarks * cxLeftRight).Count > 0)
					{
						sanitizers.Add(cxRight);
						break;
					}
				}
				left = ((BinaryExpr) left).Left;
			}
			CxList cxLeft = All.FindById(left.NodeId);
		
			if ((stringsWithQuestionMarks * cxLeft).Count > 0)
			{
				sanitizers.Add(cxRight);
			}
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

result.Add(sanitizers);