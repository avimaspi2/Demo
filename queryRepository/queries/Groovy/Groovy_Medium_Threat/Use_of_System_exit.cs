CxList exit = All.FindByMemberAccess("System.exit") + All.FindByName("exit");
CxList exitInMain = exit.GetByAncs(All.FindByType(typeof(MethodDecl)).FindByShortName("main"));

CxList relevantExit = exit - exitInMain;
foreach(CxList curExit in relevantExit)
{
	CxList prms = All.GetParameters(curExit).FindByType(typeof(IntegerLiteral));
	if(prms.Count == 1)
	{
		result.data.AddRange(curExit.data);
	}
}