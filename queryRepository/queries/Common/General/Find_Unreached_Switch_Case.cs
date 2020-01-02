CxList Case = Find_Cases();
CxList Switch = Find_SwitchStmt();
CxList caseValues = All.FindByFathers(Case);
caseValues -= caseValues.FindByType(typeof(StatementCollection));
CxList switchValues = All.FindByFathers(Switch) - Case;
foreach(CxList s in Switch)
{
	CxList cases = Case.FindByFathers(s);
	CxList switchVal = switchValues.FindByFathers(s);
	Expression switchExp = null;
	foreach(CxList se in switchVal) {
		switchExp = se.TryGetCSharpGraph<Expression>();
		if (switchExp != null) {
			break;
		}
	}
	foreach(CxList c in cases){
		CxList caseVal = caseValues.FindByFathers(c);
		Expression expCase = caseVal.TryGetCSharpGraph<Expression>();
		if(expCase == null) {
			continue;
		}
		int caseIntegerValue = 0;
		if(int.TryParse(expCase.Text, out caseIntegerValue)){
			IntegerIntervalAbstractValue caseConditionAbsValue = new IntegerIntervalAbstractValue(caseIntegerValue);
			CxList temp = switchVal.FindByAbstractValue(abstractValue => caseConditionAbsValue.IncludedIn(abstractValue));               
			if (temp.Count == 0)
			{
				result.Add(c);
			}
			else if(caseConditionAbsValue.Contains(switchExp.AbsValue))
			{
				result.Add(cases - c);
				break;
			}
		}
	}
}