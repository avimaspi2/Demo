List<string> methodsNames = new List<string>{
		"*Credit*","*credentials*","*secret*","*Account*",
		"DOB","*SSN","SSN*","*SocialSecurity*","auth*"};

CxList tempResult = All.FindByShortNames(methodsNames, false);
tempResult -= All.FindByShortName("*className*", false);

result = Find_Passwords();

foreach (CxList p in tempResult)
{
	CSharpGraph g = p.GetFirstGraph();
	if(g == null || g.ShortName == null)
	{
		continue;
	}
	if (g.ShortName.Length < 20)
	{
		result.Add(p);
	}
}