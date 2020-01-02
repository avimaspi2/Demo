CxList attrDecl = Lightning_Get_Attribute_Declarations();
 CxList allExprInProj=Lightning_Find_All_Expressions_In_Project();
foreach(CxList declaration in attrDecl)
{
	try{
		CxList elementOfDeclarator = cxXPath.GetElementOfCreatedDeclaration(declaration);
		if(declaration.GetFirstGraph().LinePragma.FileName == null)
		{
			continue;
		}
		string fileName = declaration.TryGetCSharpGraph<CSharpGraph>().LinePragma.FileName;
		CxList allExpressionsInFile = allExprInProj.FindByFileName(fileName);	
		CxList descendants = cxXPath.GetAllExpressionDescendents(allExpressionsInFile, 8);				
		allExpressionsInFile.Add(descendants.FindByType(typeof(MemberAccess)));		
		CxList sameName = allExpressionsInFile.FindByShortName(declaration.GetName());		
		CustomFlows.AddFlow(elementOfDeclarator, sameName);			
	}catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}