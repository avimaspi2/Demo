CxList Case = Find_Cases();
CxList Switch = Find_SwitchStmt();
CxList caseValues = All.FindByFathers(Case);
caseValues -= caseValues.FindByType(typeof(StatementCollection));
CxList switchValues = All.FindByFathers(Switch) - Case;

foreach (CxList caseValue in caseValues)
{
	try
	{
		CxList switchValue = switchValues.GetByAncs(caseValue.GetFathers().GetFathers());
		if (switchValue.FindByShortName(caseValue).Count > 0)
		{
			result.Add(switchValue.Concatenate(caseValue));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}