// Personal List
CxList personalList = All.FindByShortNames(new List<string> {
		"*credentials*", "DOB", "*secret*",
		"*SSN", "SSN*", "*SocialSecurity*", 
		/*"user", *//*"userName",*/ "\"auth*"}, false);

foreach (CxList p in personalList)
{
	CSharpGraph g = p.data.GetByIndex(0) as CSharpGraph;
	if(g != null && g.ShortName != null && g.ShortName.Length < 20)
	{
		result.Add(p);
	}
}