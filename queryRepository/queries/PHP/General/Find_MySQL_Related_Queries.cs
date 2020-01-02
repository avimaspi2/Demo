if(param.Length == 1)
{
	try{
		string mySql = "mysql*";
		CxList methods = Find_Methods();
		CxList mySqlProcedural = methods.FindByShortName(mySql);
		CxList procedural = All.NewCxList();
		CxList objectCreateExpr = All.FindByType(typeof(ObjectCreateExpr));
		CxList mySQLObjectObjectCreate = objectCreateExpr.FindByShortName(mySql);
		CxList fetcherMethods = All.NewCxList();
		if(param[0] != null)
		{
			List < string > fetchers = param[0] as List <string>;
			procedural.Add(mySqlProcedural.FindByShortNames(fetchers));		
			fetcherMethods.Add(methods.FindByShortNames(fetchers));
		}
		result.Add(procedural);
		CxList objectOriented = fetcherMethods.GetTargetOfMembers();
		CxList oo = objectOriented.DataInfluencedBy(mySQLObjectObjectCreate);
		result.Add(oo.GetMembersOfTarget());
	

	
	}catch(Exception e)
	{
		cxLog.WriteDebugMessage(e);	
	}
	
}