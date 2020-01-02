CxList tempResult = All.FindByShortNames(new List<String>()
	{ "*Account*", "*Credit*", "*credentials*", "DOB", "*secret*", "*SSN", "SSN*", "*SocialSecurity*", 
		//	"user"
		//	"userName"
		"auth*"}, false);

tempResult -= All.FindByShortNames(new List<String>()
	{"author", "author_*", "authors", "authors_*" ,"authorurl", "authoruri", "*className*" }, false);

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