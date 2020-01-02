//This query finds whether actionpoller in page  file is used with an interval that is less than 60
CxList interval = All.FindByMemberAccess("actionPoller.interval",  false);
CxList integerLiteral = All.FindByType(typeof(IntegerLiteral));
CxList rightSide = integerLiteral.GetByAncs(interval.GetFathers());
CxList number = rightSide.FindByAssignmentSide(CxList.AssignmentSide.Right);
foreach(CxList num in number)
{
	IntegerLiteral lit = num.data.GetByIndex(0) as IntegerLiteral;
	if(lit != null)
	{              
		if(lit.Value < 60)
		{
			result.Add(num);
		}
	}
}