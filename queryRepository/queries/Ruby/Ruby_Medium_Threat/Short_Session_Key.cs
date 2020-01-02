CxList session = All.FindByShortName("session");
CxList assignSession = session.GetAncOfType(typeof(AssignExpr));

CxList secret = All.FindByShortName("secret").FindByType(typeof(UnknownReference));
secret = secret.GetByAncs(assignSession);
CxList assignSecret = secret.GetAncOfType(typeof(AssignExpr));

CxList assignStrings = Find_Strings().GetByAncs(assignSecret);
CxList relevantSecrets = assignStrings.DataInfluencingOn(secret);

foreach (CxList secretName in relevantSecrets)
{
	StringLiteral s = secretName.data.GetByIndex(0) as StringLiteral;
	if (s != null)
	{
		string name = s.ShortName;
		if (name.Length < 32) // 30 recommended + 2 for the quotes of the string
		{
			result.Add(secretName);
		}
	}
}