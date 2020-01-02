CxList declarators = Lightning_Get_Attribute_Declarations();
//client side inputs will be only public attributes or global attributes
//and will be private attributes that are assigned using inputText

//get all non private attributes
CxList vdStmt=declarators.GetAncOfType(typeof(VariableDeclStmt));
CxList privateAttributes= vdStmt.FindByFieldAttributes(Modifiers.Private);
CxList allOtherThanPrivate=vdStmt - privateAttributes;
result=cxXPath.GetElementOfCreatedDeclaration(declarators.GetByAncs(allOtherThanPrivate));

//find all private attributes modified by inputText
CxList privateDeclarators=declarators.GetByAncs(privateAttributes);
CxList declaratorsAsNodes = cxXPath.GetElementOfCreatedDeclaration(privateDeclarators);
CxList allExprInProj=Lightning_Find_All_Expressions_In_Project();
CxList relevant = All.NewCxList();
foreach(CxList expression in allExprInProj)
{
	CxList allRelatedElements = cxXPath.GetElementByExpression(expression);
	CxList inputText = allRelatedElements.FindByShortName("inputText");
	if(inputText.Count > 0)
	{
		relevant.Add(expression);
	}
}
result.Add(declaratorsAsNodes.DataInfluencingOn(relevant).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));