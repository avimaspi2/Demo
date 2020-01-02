/* Link Control Element Attributes to the set methods in the Control class*/
if(cxScan.IsFrameworkActive("SAPUI")) {
	CxList assocArrayExprs = Find_AssociativeArrayExpr();
	CxList lambdas = Find_LambdaExpr();
	CxList declarators = Find_Declarators();
	CxList parameters = Find_Parameters();
	CxList paramDecls = Find_ParamDecl();

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

	foreach(CxList memberAccess in memberAccessesInsideIf) {
		string[] controlNameList = memberAccess.GetName().Split('_');
		string controlName = controlNameList[controlNameList.Length - 1];
		CxList target = memberAccess.GetMembersOfTarget();
		string targetName = target.GetName();
		CxList setMethod = All.NewCxList();
		try {
			setMethod = lambdas.GetByAncs(declarators
				.GetByAncs(dict[controlName]).FindByShortName("set" + targetName, false));
		} catch {
			continue;
		}
		if(setMethod.Count == 0) continue;
		CxList methodParameter = paramDecls.GetParameters(setMethod, 0);
		CxList cxOutput = target.GetAssigner();
		if(cxOutput.GetName().Equals("CxOutput")) {
			cxOutput = All.GetParameters(cxOutput, 0) - parameters;
		}
		CustomFlows.AddFlow(cxOutput, methodParameter);
	}
}