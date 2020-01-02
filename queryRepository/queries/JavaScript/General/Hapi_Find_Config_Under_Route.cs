/* This query gets Route Class Decl DOM-Object, and returns it's Config Class Decl - DOM-Object  */
if(param.Length > 0)
{
	try
	{
		CxList methodDecls = Find_MethodDecls();
		
		CxList mba = Find_MemberAccesses();
		CxList unkr = Find_UnknownReference();
		
		CxList mbaUnkr = All.NewCxList();
		mbaUnkr.Add(mba);
		mbaUnkr.Add(unkr);
		
		CxList methodDeclsMba = All.NewCxList();
		methodDeclsMba.Add(methodDecls);
		methodDeclsMba.Add(mba);
		
		CxList routeClassDecl = param[0] as CxList;
		if(routeClassDecl != null)
		{
			CxList allUnderRouteClassDecl = unkr.GetByAncs(routeClassDecl);

			CxList toBeResolvedConfigs = allUnderRouteClassDecl.FindByShortName("config");
			CxList configClassDecls = Get_Class_Of_Anonymous_Ref(toBeResolvedConfigs);
			CxList rightSideConfig = mbaUnkr.FindByAssignmentSide(CxList.AssignmentSide.Right)
				.GetByAncs(toBeResolvedConfigs.GetFathers());

			rightSideConfig = methodDeclsMba.FindByShortName(rightSideConfig);
			CxList relevantOC = Find_ObjectCreations().GetByAncs(rightSideConfig.GetFathers().FindByType(typeof(AssignExpr)));
			CxList cdecl = Find_Classes().FindByShortName(relevantOC);
			configClassDecls.Add(cdecl);
			result = configClassDecls;	
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}