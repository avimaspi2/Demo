// Credit Card List
CxList creditList = All.FindByShortNames(new List<string>{
		"AccountNumber", "AccountNo", "AccountId",
		"*CCN", "CCN*", "*Credit*", "*CreditCard*"}, false);


foreach (CxList p in creditList)
{
	CSharpGraph g = p.TryGetCSharpGraph<CSharpGraph>();
	if(g != null && g.ShortName != null && g.ShortName.Length < 20)
	{
		result.Add(p);
	}
}