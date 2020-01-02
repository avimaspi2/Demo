//create data flows inside the component iteration statement
CxList iterations = cxXPath.CreateIterationVarDefinition("*.cmp", 8, "Lightning");
CxList allExpressionsInProject = Lightning_Find_All_Expressions_In_Project();
foreach(CxList iteration in iterations)
{	
	
	CxList elementOfDeclarator = cxXPath.GetElementOfCreatedDeclaration(iteration);
	CxList desc = cxXPath.GetXMLNodeDescendents(elementOfDeclarator, allExpressionsInProject);
	CustomFlows.AddFlow(elementOfDeclarator, desc);	

	//now link expression to declaration.		

	foreach(CxList element in elementOfDeclarator)
	{
		try{
			CxXmlNode attrAsNode = element.TryGetCSharpGraph<CxXmlNode>();
			if(attrAsNode != null)
			{
				CxList foundExpression = All.NewCxList();
				Expression e = attrAsNode.GetAttributeValueExpressionByName("items");			
				if(e != null)
				{
				
					foundExpression.Add(e.NodeId, e);			
					CustomFlows.AddFlow(foundExpression, elementOfDeclarator);								
				}
			}
		}catch(Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
	}		
}