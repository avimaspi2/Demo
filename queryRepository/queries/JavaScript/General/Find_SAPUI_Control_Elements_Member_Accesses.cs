/* Get all Member Accesses inside If statement that repserents a Control Element */
if(cxScan.IsFrameworkActive("SAPUI")) {
	CxList ifStatements = Find_Ifs();
	CxList unknownReferences = Find_UnknownReference();
	CxList memberAccesses = Find_MemberAccesses();
	CxList memberAccessesAndUnkRef = unknownReferences.Clone();
	memberAccessesAndUnkRef.Add(memberAccesses);

	CxList uiExtend = Find_SAPUI_Extending_Objects("sap/ui/core/Control", true);

	CxList controlStringLiterals = All.GetParameters(uiExtend, 0).FindByType(typeof(StringLiteral));

	List<string> shortNames = new List<string> ();

	foreach(CxList control in uiExtend) {
		string controlName = controlStringLiterals.GetParameters(control, 0).GetName();
		if(!String.IsNullOrEmpty(controlName)) {
			string[] splittedString = controlName.Split('.');
			string name = splittedString[splittedString.Length - 1].ToLower();
			shortNames.Add("*_" + name);
		}
	}

	CxList controlElements = memberAccessesAndUnkRef.FindByShortNames(shortNames,false);
	CxList controlElementsIfStmt = controlElements.GetByAncs(ifStatements).GetFathers();
	CxList memberAccessesInsideIf = controlElements.GetByAncs(controlElementsIfStmt.GetBlocksOfIfStatements(true));
	CxList idMemberaccesses = memberAccessesInsideIf.GetMembersOfTarget().FindByShortName("id",false).GetTargetOfMembers();

	result = memberAccessesInsideIf - idMemberaccesses;
}