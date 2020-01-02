/// <summary>
/// This query checks if a switch contains a case, which is identical to the switch condition.
/// If this exists, all cases below this case won't be reachable.
/// </summary>

CxList Switch = base.Find_SwitchStmt();
CxList Case = base.Find_Cases();

CxList switchValues = All.FindByFathers(Switch) - Case;
CxList caseValues = All.FindByFathers(Case);
caseValues -= caseValues.FindByType(typeof(StatementCollection));

//In case of MemberAccess, we only want the rightmost MemberAccess and remove everything else
CxList newCaseValues = All.NewCxList();
foreach (CxList cV in caseValues)
{
	/*If the element (UnknownRef, MemberAccess, etc) has no Rightmost member it means
	  that the element is itself the Rightmost member, so we keep it. Otherwise we discard it*/
	if (cV.GetRightmostMember().Count == 0)
	{
		newCaseValues.Add(cV);
	}
}

foreach (CxList caseValue in newCaseValues)
{
	// Validate case and switch are looking at the same type of parameter (String/Variable/Other) and that the case
	// is identical to the switch statement.
	CxList _switchValues = switchValues.GetByAncs(caseValue.GetFathers().GetFathers());
	
	//Same as above
	CxList switchValue = All.NewCxList();
	foreach (CxList sV in _switchValues)
	{
		if (sV.GetRightmostMember().Count == 0)
		{
			switchValue.Add(sV);
		}
	}

	CSharpGraph switchGraph = switchValue.TryGetCSharpGraph<CSharpGraph>();
	CSharpGraph caseGraph = caseValue.TryGetCSharpGraph<CSharpGraph>();

	if ((switchGraph.GetType() == caseGraph.GetType()) && (switchValue.FindByName(caseValue).Count > 0))
	{
		result.Add(switchValue.Concatenate(caseValue));
	}
}