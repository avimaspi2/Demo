CxList relevantExit = Find_Use_Of_System_Exit();
foreach(CxList curExit in relevantExit)
{
	CxList prms = All.GetParameters(curExit).FindByType(typeof(IntegerLiteral));
	if(prms.Count == 1)
	{
		result.data.AddRange(curExit.data);
	}
}