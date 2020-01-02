CxList Case = All.FindByType(typeof(Case));
CxList Switch = All.FindByType(typeof(SwitchStmt));
CxList caseValues = All.FindByFathers(Case);
caseValues -= caseValues.FindByType(typeof(StatementCollection));
CxList switchValues = All.FindByFathers(Switch) - Case;

foreach (CxList caseValue in caseValues)
{
	CxList switchValue = switchValues.GetByAncs(caseValue.GetFathers().GetFathers());
	if (switchValue.FindByShortName(caseValue).Count > 0)
	{
		result.Add(switchValue.Concatenate(caseValue));
	}
}