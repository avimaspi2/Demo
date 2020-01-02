//
//Finds methods and members access of import file
//Input: module - Import file to search
//       methods - List of methods or members access to search
//       allImports (Optional) - All imports, which are relevant for searching the module
//		 unknownReferences (Optional) - List of unknown references to search. Pass null as paramter in allImports
//										when you need to pass this argument
// Return: All methods/member access occurrences.
try
{ 
	if (param.Length > 1 && param.Length < 5)
	{
		string module = param[0] as string;
		String[] methods = param[1] as string[]; 
		CxList allImports = (param.Length == 3) ?  param[2] as CxList : Find_Imports();
		CxList unknownRefs = (param.Length == 4) ? param[3] as CxList : All.NewCxList();
    
		//Find all modules imports
		CxList imports = allImports.FindByName(module);
        
		CxList allMethods = Find_Methods(); 
		CxList memberAccess = Find_MemberAccesses();
		CxList search = All.NewCxList();
		search.Add(allMethods);
		search.Add(memberAccess);
		
		CxList irrelevantUnkRef = All.NewCxList();
		foreach(CxList unkRef in unknownRefs) {
			if(module.Equals(unkRef.GetName())) {
				irrelevantUnkRef.Add(unkRef);
			}
		}
		search.Add(unknownRefs - irrelevantUnkRef);
        

		CxList all = All.NewCxList();
        
		CxList relevantMethods = All.NewCxList();
		CxList foundByParam = All.NewCxList();
		CxList functions_renamed = All.NewCxList();
        
		foreach(String importedMethod in methods){
			relevantMethods.Add(search.FindByName(importedMethod));
			foundByParam.Add(Find_By_Param_Name(importedMethod));
			functions_renamed.Add(allImports.FindByName(module + "." + importedMethod));
		}
		//Find all the function occurrences
		foreach(CxList item in imports)
		{
			Import im = item.TryGetCSharpGraph<Import>();
			if(im == null || im.FullName == null)
			{
				continue;
			}
			int fileId = im.LinePragma.GetFileId();
			CxList elements = search.FindByFileId(fileId);
			
			CxList methodsParams = All.NewCxList();
			methodsParams.Add(relevantMethods);
			methodsParams.Add(foundByParam);
			
			result.Add(methodsParams.FindByFileId(fileId));
			string aliasOfImport = im.NamespaceAlias;
			foreach(String m in methods){
                
				if(aliasOfImport != null) {
					result.Add(elements.FindByMemberAccess(aliasOfImport, m));
				} else {
					result.Add(elements.FindByMemberAccess(im.FullName, m));
				}
			}
    
            
		}
		foreach(CxList f in functions_renamed)
		{
			Import im_f = f.TryGetCSharpGraph<Import>();
			if (im_f != null && im_f.LinePragma != null && im_f.NamespaceAlias != null)
			{
				CxList elements_f = search.FindByFileId(im_f.LinePragma.GetFileId());                       
				result.Add(elements_f.FindByName(im_f.NamespaceAlias));
			}
		}
        
		//find all methods without import
		foreach(string m in methods)
		{
			all.Add(allMethods.FindByName(module + "." + m));
		}
        
		result.Add(all);
	}
} 
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}