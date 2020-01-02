/* Get CxOutputs inside Control Element Attributs that has a set method in Control class declared
 * Those CxOutputs are not sinks.*/ 
if(cxScan.IsFrameworkActive("SAPUI"))
{
	CxList assocArrayExprs = Find_AssociativeArrayExpr();
	CxList fieldDecls = Find_FieldDecls();
	CxList uiExtend = Find_SAPUI_Extending_Objects("sap/ui/core/Control", true);
	
	CxList associativeArrayExprs = assocArrayExprs.GetParameters(uiExtend, 1);
	CxList controlStringLiterals = All.GetParameters(uiExtend, 0).FindByType(typeof(StringLiteral));
	
	Dictionary<string, CxList> dict = new Dictionary<string, CxList>();
	
	foreach(CxList control in uiExtend) {
		CxList associativeArray = associativeArrayExprs.GetParameters(control, 1);
		string controlName = controlStringLiterals.GetParameters(control, 0).GetName();
		if(!String.IsNullOrEmpty(controlName)) {
			string[] splittedString = controlName.Split('.');
			string name = splittedString[splittedString.Length - 1].ToUpper();
			if(dict.ContainsKey(name))
			{
				dict[name].Add(associativeArray);
			}
			else
			{
				dict.Add(name, associativeArray);
			}
		}
	}
	
	CxList memberAccessesInsideIf = Find_SAPUI_Control_Elements_Member_Accesses();
	
	foreach(CxList member in memberAccessesInsideIf) {
		string[] controlSplitted = member.GetName().Split('_');
		CxList target = member.GetMembersOfTarget();
		try {
			CxList assocArray = dict[controlSplitted[controlSplitted.Length - 1]];
			CxList fieldDecl = fieldDecls.GetByAncs(assocArray).FindByShortName("set" + target.GetName(), false);
			if(fieldDecl.Count > 0)
				result.Add(target.GetAssigner().FindByShortName("CxOutput"));
		} catch {
			continue;
		}
	}
}