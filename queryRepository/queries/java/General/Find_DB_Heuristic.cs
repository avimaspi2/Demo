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
	CSharpGraph gr = sql.TryGetCSharpGraph<CSharpGraph>();
	string name = gr.ShortName.TrimStart(trimChars);
	if (name.ToLower().StartsWith("select"))
	{
		SQLLines.Add(gr.NodeId, gr);
	}
}

SQL = strings.FindByName("*update *", false);
foreach (CxList sql in SQL)
{
	CSharpGraph gr = sql.TryGetCSharpGraph<CSharpGraph>();
	string name = gr.ShortName.TrimStart(trimChars);
	if (name.ToLower().StartsWith("update"))
	{
		SQLLines.Add(gr.NodeId, gr);
	}
}

SQLLines.Add(strings.FindByName("*insert into*", false));
SQLLines.Add(strings.FindByName("*delete from*", false));

CxList potentialDB = Find_DB_Methods();
CxList dbIn = Find_DB_In();
CxList dbOut = Find_DB_Out();

potentialDB -= dbIn;
potentialDB -= potentialDB.FindByParameters(dbOut);
result = potentialDB * potentialDB.DataInfluencedBy(SQLLines);
CxList methods = Find_Methods();
result.Add(All.GetParameters(methods.FindByShortName("execute*", false), 0));

CxList dbInAndOut = All.NewCxList();
dbInAndOut.Add(dbIn);
dbInAndOut.Add(dbOut);

result -= All.GetParameters(dbInAndOut,0);

if (result.Count > 0)
{
	result -= result.DataInfluencedBy(result);
	result -= result.DataInfluencedBy(dbOut);
}