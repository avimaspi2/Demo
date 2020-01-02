CxList tempResult = 
//	All.FindByShortName("*Account*", false) + in Apex should be removed
	All.FindByShortName("*Credit*", false) + 
	All.FindByShortName("*credentials*", false) + 
	All.FindByShortName("*secret*", false) + 
	All.FindByShortName("*SSN", false) +
	All.FindByShortName("SSN*", false) +
	All.FindByShortName("DOB", false) +
//	All.FindByShortName("user", false) +
//	All.FindByShortName("username", false) +
	All.FindByShortName("*SocialSecurity*", false) +
	All.FindByShortName("auth*", false);

tempResult -= tempResult.FindByShortName("*className*", false);
tempResult -= tempResult.FindByShortName("is*");
tempResult -= tempResult.FindByShortName("setis*",false);
tempResult -= tempResult.FindByShortName("getis*", false);

result = Find_Passwords();

foreach (CxList p in tempResult)
{
	CSharpGraph g = p.data.GetByIndex(0) as CSharpGraph;
	if(g == null || g.ShortName == null)
	{
		continue;
	}
	if (g.ShortName.Length < 20)
	{
		result.Add(p);
	}
}