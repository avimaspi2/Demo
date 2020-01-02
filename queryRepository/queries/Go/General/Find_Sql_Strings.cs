Func <CxList, string, CxList > getQueries = delegate(CxList forSearch, string typeSearch){
	CxList toReturn = All.NewCxList();
	char[] trimChars = new char[7] {' ', '\t', '"', '(', '\r', '\n', '`'};
	foreach (CxList sql in forSearch)
	{
		CSharpGraph gr = sql.TryGetCSharpGraph<CSharpGraph>();
		string name = gr.ShortName.TrimStart(trimChars);
		if (name.ToLower().StartsWith(typeSearch))
		{
			toReturn.Add(sql);
		}
	}
	return toReturn;
};


CxList strings = Find_Strings();
CxList removeList = strings.FindByName("* the *", false);// try to remove English sentences
removeList.Add(strings.FindByName("* this *", false));// try to remove English sentences
removeList.Add(strings.FindByName("* you *", false));// try to remove English sentences
removeList.Add(strings.FindByName("* your *", false));// try to remove English sentences
removeList.Add(strings.FindByName("", false));
removeList.Add(strings.FindByName(" ", false));

strings -= removeList;


CxList SQLLines = All.NewCxList();

CxList SQL = strings.FindByName("*select *", false);
SQLLines.Add(getQueries(SQL,"select"));

SQL = strings.FindByName("*update *", false);
SQLLines.Add(getQueries(SQL,"update"));


SQLLines.Add(strings.FindByName("*insert into*", false));
SQLLines.Add(strings.FindByName("*delete from*", false));
result = SQLLines;