CxList tempResult = All.FindByShortNames(new List<string>(new string[] {
	"*Credit*","*credentials*","*secret*","*Account*","DOB","*SSN","SSN*","*SocialSecurity*","\"auth*"}), false);

tempResult -= All.FindByShortName("*className*", false);

result = Find_Passwords();

foreach (CxList p in tempResult)
{
	try{
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
	catch (Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}