/*
	Description:	Return all methods (with or without alias) from referred imports used
	Input:	module - Import file to search
			methods - List of methods or members access to search
			allImports (Optional) - All imports, which are relevant for searching the module
	Return: All methods/member access occurrences.
*/
try {
	if (param.Length > 1 && param.Length < 4)
	{
		
		string packageName = param[0] as string;
		String[] methods = param[1] as string[]; 
		CxList allImports = (param.Length == 3) ? param[2] as CxList : Find_Imports();
 
		CxList allMethods = Find_Methods();

		CxList members = Find_MemberAccesses();
		members.Add(allMethods.GetTargetOfMembers().GetMembersOfTarget()); 
		CxList membersMethods = All.NewCxList();
		foreach(string methodName in methods)
		{
			membersMethods.Add(members.FindByMemberAccess("*." + methodName));
		}
		foreach(CxList item in allImports) 
		{
			Import import = item.TryGetCSharpGraph<Import>();
			if(import == null || import.FullName == null)
			{
				continue;
			}
		
			string importName = import.FullName.Replace("\"", "");		
		
			if (packageName != importName){
				continue;
			}
		
			/* block to solve the import's alias */
			if (importName.Contains(".")) {
				int idx = importName.IndexOf('.');
				importName = importName.Substring(idx + 1) + '"';
			}else{
				importName = importName + '"';
			}
			/* /block */

			CxList relevantMembers = membersMethods.FindByMemberAccess(importName + ".*");
			foreach(string methodName in methods){
				result.Add(relevantMembers.FindByMemberAccess(importName, methodName));
			}
		}	

	}else{
		cxLog.WriteDebugMessage("Number of parameters should be 2 or 3");			
	}
}
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}