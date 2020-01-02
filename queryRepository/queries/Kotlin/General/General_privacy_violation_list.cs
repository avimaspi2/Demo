// Personal List
CxList personalList = All.FindByShortNames(new List<string> {
		"*credentials*", "*secret*",
		"*SSN", "SSN*", "DOB", "*SocialSecurity*", "\"auth*"}, false);

// If this is an Android project then add Android personal info
if(Find_Android_Settings().Count > 0)
{
	personalList.Add(Find_Android_Personal_Info());
}


foreach (CxList p in personalList)
{
	CSharpGraph g = p.TryGetCSharpGraph<CSharpGraph>();
	if(g != null && g.ShortName != null && g.ShortName.Length < 20)
	{
		result.Add(p);
	}
}