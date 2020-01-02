try
{
	if (param.Length > 1 && param.Length < 4)
	{
		string module = param[0] as string;
		String modelDef = param[1] as string; 
		CxList allImports = (param.Length == 3) ? param[2] as CxList : Find_Imports();
		
		//Find all modules imports
		CxList imports = allImports.FindByName(module);
		CxList classDecl = All.NewCxList();
		
		foreach(CxList item in imports){
			Import im = item.TryGetCSharpGraph<Import>();
			
			CxList elements = All.FindByFileName(im.LinePragma.FileName);
					
			CxList model = elements.FindByShortName(modelDef);
			
			classDecl.Add(model.FindByShortName(modelDef));
			if(im.NamespaceAlias != null) {
				classDecl.Add(model.FindByMemberAccess(im.NamespaceAlias, modelDef));
			} else {
				classDecl.Add(model.FindByMemberAccess(im.FullName, modelDef));
			}
		} 
		
		//Find all data influenced by the models
		result.Add(classDecl.ReduceFlowByPragma().GetFathers().GetFathers());
	}
} 
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}