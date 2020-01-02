CxList tempResult = All.FindByShortNames(new List<string> {
		"*Credit*",
		"*credentials*",
		"*secret*",
		"*Account*",
		"*SSN",
		"DOB",
		"SSN*",
		"*SocialSecurity*",
		"DeviceUniqueId",
		// "user",
		// "userName",
		"auth*"}, false);

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