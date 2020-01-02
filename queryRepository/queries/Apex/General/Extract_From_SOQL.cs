if (param.Length >= 2)
{
	string keyword = param[0] as string;
	string contains = param[1] as string;
	
	CxList dbQuery = All.FindByMemberAccess("database.query");
	CxList strings = Find_Strings();
	CxList soqlStrings = strings.GetByAncs(All.GetParameters(All.FindByMemberAccess("Cx_VirtualDal.select")));
	soqlStrings.Add(All * strings.DataInfluencingOn(dbQuery));

	System.Collections.Generic.List<string> whereParameters = new System.Collections.Generic.List<string>();
	foreach (CxList curSOQL in soqlStrings)
	{
		whereParameters = curSOQL.ExtractFromSOQL(keyword);
		if (param.Length == 3 && whereParameters.Contains(param[2] as string)) 
		{
			result.Add(curSOQL);
		}
		else
		{
			foreach(string inWhere in whereParameters) 
			{
				if (inWhere.Contains(contains)) 
				{
					result.Add(curSOQL);
					break;
				}
			}
		}
	}
}