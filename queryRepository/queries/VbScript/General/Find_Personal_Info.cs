CxList tempResult = 
	All.FindByShortName("*Credit*", false) + 
	All.FindByShortName("*credentials*", false) + 
	All.FindByShortName("*secret*", false) + 
	All.FindByShortName("*Account*", false) + 
	All.FindByShortName("*SSN", false) +
	All.FindByShortName("SSN*", false) +
	All.FindByShortName("DOB", false) +
	All.FindByShortName("*SocialSecurity*", false) +
//	All.FindByShortName("user", false) +
//	All.FindByShortName("userName", false) +
	All.FindByShortName("auth*", false);

tempResult -= All.FindByShortName("*className*", false);

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