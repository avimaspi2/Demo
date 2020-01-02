CxList cmpOrApp = Lightning_Find_Aura_Cmp_And_App();
CxList attrDeclarations = Lightning_Get_Attribute_Declarations();

CxList attrs = cxXPath.FindAllAttributesThatHoldExpressions("*.cmp", 8, "Lightning");
CxList allExpressions = cxXPath.GetExpressionsByAttributes(attrs);
List<string> fileNamesList = new List<string>();

foreach(CxList cmp in cmpOrApp)
{
	CSharpGraph g = cmp.TryGetCSharpGraph<CSharpGraph>();

	if(g != null && g.LinePragma != null)
	{
		string fileName = g.LinePragma.FileName;
		string onlyFileName = fileName.Remove(0, fileName.LastIndexOf("\\") + 1);
		onlyFileName = onlyFileName.Remove(onlyFileName.LastIndexOf("."), 4);
		if(!fileNamesList.Contains(onlyFileName))
		{
			fileNamesList.Add(onlyFileName);
		}
	}	
}
CxList foundInclude = All.NewCxList();



foreach(string file in fileNamesList)
{
	foundInclude.Add(cxXPath.FindXmlNodesByQualifiedName("*.cmp", 8, "c", file, true));
	
}



foreach(CxList include in foundInclude)
{	
	string fileName = "*" + include.GetName() + ".cmp";
	CxList attributesDefinedInIncluded = attrDeclarations.FindByFileName(fileName);
	CxXmlNode node = include.TryGetCSharpGraph<CxXmlNode>();
	List<string> attributesNames = node.GetNamesOfAttributes();
	
	foreach(string attr in attributesNames)
	{
		
		Expression e = node.GetAttributeValueExpressionByName(attr);
		if(e != null)
		{
			CxList parameter = allExpressions.FindById(e.NodeId);
			CxList paramDecl = attributesDefinedInIncluded.FindByShortName(attr);
			CxList elementOfDeclarator = cxXPath.GetElementOfCreatedDeclaration(paramDecl);			
			CustomFlows.AddFlow(parameter, elementOfDeclarator);
		}
	}
}