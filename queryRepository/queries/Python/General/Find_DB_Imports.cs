try
{
	if (param.Length == 4 )
	{
		String[] modules = param[0] as string[];
		String[] connStrings = param[1] as string[];
		CxList imports = param[2] as CxList;
		string modelDef = param[3] as string;
	
		//Find all DB imports
		CxList importsSQL = All.NewCxList();

		foreach(String m in modules){
			importsSQL.Add(imports.FindByName(m));
		}
	
		//Find all data influenced by DB connection
		CxList connections = All.NewCxList();

		//Find all the class declarations
		CxList classDecl = All.NewCxList();

		foreach(CxList item in importsSQL){
			try 
			{
				Import im = item.TryGetCSharpGraph<Import>();
		
				CxList elements = All.FindByFileName(im.LinePragma.FileName);
				CxList conn = elements.FindByType(typeof(MethodInvokeExpr));
				
				foreach(String s in connStrings){				
					
					connections.Add(conn.FindByName(s));
					connections.Add(conn.FindByMemberAccess(s));
					
					if(im.NamespaceAlias != null) {
						connections.Add(conn.FindByMemberAccess(im.NamespaceAlias, s));
					} else {
						connections.Add(conn.FindByMemberAccess(im.FullName, s));
					}
				}
				
				if (modelDef != null)
				{
					CxList model = elements.FindByShortName(modelDef);
					classDecl.Add(model.FindByShortName(modelDef));
					if(im.NamespaceAlias != null) {
						classDecl.Add(model.FindByMemberAccess(im.NamespaceAlias, modelDef));
					} else {
						classDecl.Add(model.FindByMemberAccess(im.FullName, modelDef));
					}
				}
			}
			catch (Exception ex)
			{
				cxLog.WriteDebugMessage(ex);
			}
		} 
	
		//Find all data influenced by the connection strings
		result.Add(All.InfluencedBy(connections));
		//Find all data influenced by the models
		result.Add(classDecl.GetFathers().GetFathers());
		
		//find all modules without import
		CxList methods = Find_Methods();
		foreach(string module in modules)
		{
			foreach(string c in connStrings)
			{
				result.Add(All.InfluencedBy(
					methods.FindByName(module + "." + c)));
			}
		}
	}
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
} 
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}