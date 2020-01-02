CxList strings = Find_Strings();
strings -= strings.FindByName("* the *", false); // try to remove English sentences
strings -= strings.FindByName("* this *", false); // try to remove English sentences
strings -= strings.FindByName("* you *", false); // try to remove English sentences
strings -= strings.FindByName("* your *", false); // try to remove English sentences
strings -= strings.FindByName("", false);
strings -= strings.FindByName(" ", false);

char[] trimChars = new char[6] {' ', '\t', '"', '(', '\r', '\n'};

CxList SQL = strings.FindByName("*select *", false);
CxList SQLLines = All.NewCxList();
foreach (CxList sql in SQL)
{
	CSharpGraph gr = sql.data.GetByIndex(0) as CSharpGraph;
	string name = gr.ShortName.TrimStart(trimChars);
	if (name.ToLower().StartsWith("select"))
	{
		SQLLines.Add(sql);
	}
}

SQL = strings.FindByName("*update *", false);
foreach (CxList sql in SQL)
{
	CSharpGraph gr = sql.data.GetByIndex(0) as CSharpGraph;
	string name = gr.ShortName.TrimStart(trimChars);
	if (name.ToLower().StartsWith("update"))
	{
		SQLLines.Add(sql);
	}
}

SQLLines.Add(strings.FindByName("*insert into*", false));
SQLLines.Add(strings.FindByName("*delete from*", false));

CxList potentialBD = Find_Methods();
potentialBD -= potentialBD.FindByShortName("*$_Double*");

result = potentialBD.InfluencedBy(SQLLines);