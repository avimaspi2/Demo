/*in this query we will receive two parameters and will look whether the JSON conditions object that is
assoiated with it has only one global key */
if(param.Length == 2)
{
	CxList conditions = param[0] as CxList;
	CxList xsAll = param[1] as CxList;	
		
	CxList foundVulnerableQueryParameter = All.NewCxList();
	CxList classDeclaration = xsAll.FindByType(typeof(ClassDecl));
	CxList declarator = xsAll.FindByType(typeof(Declarator));
	CxList urConditions = conditions.FindByType(typeof(UnknownReference));		
	CxList reference = xsAll.FindAllReferences(urConditions);
	CxList oce = xsAll.FindByType(typeof(ObjectCreateExpr));
	foreach(CxList variable in urConditions)
	{
		CxList foundObjectCreate = oce.GetByAncs(reference.FindAllReferences(variable).GetFathers());
		CxList relevantClass = classDeclaration.FindByShortName(foundObjectCreate);
		
		if((xsAll.FindAllMembers(relevantClass) * declarator).Count <= 1)
		{
			foundVulnerableQueryParameter.Add(variable);
		}
	}
	
	
	result = foundVulnerableQueryParameter;
}