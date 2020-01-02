CxList strings = Find_Strings();
CxList toRemove = strings.FindByName("* the *", false); // try to remove English sentences
toRemove.Add(strings.FindByName("* this *", false)); // try to remove English sentences
toRemove.Add(strings.FindByName("* you *", false)); // try to remove English sentences
toRemove.Add(strings.FindByName("* your *", false)); // try to remove English sentences
toRemove.Add(strings.FindByName("", false));
toRemove.Add(strings.FindByName(" ", false));
strings -= toRemove;

char[] trimChars = new char[6] {' ', '\t', '"', '(', '\r', '\n'};

CxList SQL = strings.FindByName("*select *", false);
CxList SQLLines = All.NewCxList();
foreach (CxList sql in SQL)
{
	try{
		CSharpGraph gr = sql.data.GetByIndex(0) as CSharpGraph;
		string name = gr.ShortName.TrimStart(trimChars);
		if (name.ToLower().StartsWith("select", StringComparison.InvariantCultureIgnoreCase))
		{
			SQLLines.Add(sql);
		}
	}
	catch (Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}

SQL = strings.FindByName("*update *", false);
foreach (CxList sql in SQL)
{
	try{
		CSharpGraph gr = sql.data.GetByIndex(0) as CSharpGraph;
		string name = gr.ShortName.TrimStart(trimChars);
		if (name.ToLower().StartsWith("update", StringComparison.InvariantCultureIgnoreCase))
		{
			SQLLines.Add(sql);
		}
	}
	catch (Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}

SQLLines.Add(strings.FindByName("*insert into*", false));
SQLLines.Add(strings.FindByName("*delete from*", false));

CxList potentialBD = Find_DB_Methods();

result = potentialBD.DataInfluencedBy(SQLLines);

if (result.Count > 0)
{
	result -= Find_DB();
	result -= result.DataInfluencedBy(result);
	result -= result.DataInfluencedBy(Find_DB());
}