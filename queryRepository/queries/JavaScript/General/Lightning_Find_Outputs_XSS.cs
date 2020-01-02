CxList allExprInProject = Lightning_Find_All_Expressions_In_Project();

foreach(CxList expression in allExprInProject)
{
	CxList attr = cxXPath.GetAttributeByExpression(expression);
	CxList elem = cxXPath.GetElementByExpression(expression);
	bool vulnerable = false;
	
	if(attr.FindByShortName("src*").Count>0 && 
		(elem.FindByShortName("iframe").Count > 0 
			|| elem.FindByShortName("embed").Count > 0))
	{
		vulnerable = true;
		
	}else if(attr.FindByShortName("href").Count > 0 && elem.FindByShortName("a").Count > 0)
	{
		vulnerable = true;
		
	}else if(attr.FindByShortName("formaction").Count > 0 && elem.FindByShortName("form").Count > 0)
	{
		vulnerable = true;
	}
	else if(attr.FindByShortName("data").Count > 0 && elem.FindByShortName("object").Count > 0)
	{
		vulnerable = true;
	}
	
	if(vulnerable)
	{
		CxList allBinaries = expression.FindByType(typeof(BinaryExpr));
		CxList members = cxXPath.GetAllExpressionDescendents(allBinaries, 8).FindByType(typeof(MemberAccess));		
		result.Add(expression - allBinaries + members);
		
	}		
}
result.Add(Find_Outputs_XSS());