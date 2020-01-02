/*CxList strings = Find_Strings();
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

return potentialBD.InfluencedBy(SQLLines);
	//SQLLines.InfluencingOn();

result = SQLLines.GetAncOfType(typeof(MethodInvokeExpr));

result.Add(result.FindByShortName("*$_Double*").GetFathers());

//result = result.FindDes
// GetAncOfType(typeof(MethodInvokeExpr));
	//.FindByType(typeof(MethodInvokeExpr));
return result;
result -= result.FindByShortName("*$_Double*");
	//.InfluencedBy(SQLLines);
	
	//.InfluencedBy(SQLLines);

return result;//.FindByType(typeof(MethodInvokeExpr));
if (result.Count > 0)
{
	result -= (Find_DB_In() + Find_DB_Out());
	result -= result.DataInfluencedBy(result);
	result -= result.DataInfluencedBy(Find_DB_In() + Find_DB_Out());
}*/
result = All;