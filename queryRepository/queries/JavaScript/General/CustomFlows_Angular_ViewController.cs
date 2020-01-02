// Create view - controller custom flows (Angular)
// This query creates custom flows from the view outputs to the corresponding object in the controller,
// by mapping them by the shortName and members 
CxList unkRefs = Find_UnknownReference();
CxList assignExpr = Find_AssignExpr();
CxList memberAccess = Find_MemberAccesses();

CxList components = Find_CustomAttribute().FindByShortName("Component");

CxList htmlFiles = (unkRefs.GetByAncs(assignExpr.GetByAncs(components))).FindByShortName("templateUrl").GetAssigner();

foreach (CxList htmlFile in htmlFiles) 
{	
	string file = string.Format("*{0}", htmlFile.TryGetCSharpGraph<StringLiteral>().Value.Substring(2));
	string[] fileParts = file.Split('.');
	fileParts[fileParts.Length - 1] = "ts";
	string controllerFile = String.Join(".", fileParts);
	
	CxList output = unkRefs.FindByFileName(file);
	
	foreach (CxList outp in output)
	{	
		CxList controllerRefs = memberAccess.FindByFileName(controllerFile).FindByShortName(outp);				
		CxList controllerRefsWithMembers = controllerRefs.GetTargetsWithMembers();
		
		if(controllerRefsWithMembers.Count > 0){
			CxList outpMembers = outp.GetMembersOfTarget();
			CxList controllerRefsMembers = controllerRefsWithMembers.GetMembersOfTarget().FindByShortName(outpMembers);
			
			if(controllerRefsMembers.Count > 0){
				CustomFlows.AddFlow(controllerRefsMembers, outpMembers);
			}
		}		
		else{
			CustomFlows.AddFlow(controllerRefs, outp);
		}
	}
}