//Find SOQL statements where "where" clause criteria evaluates to null, more specifically
//check if a variable dynamically MAY evaluate to a null

//Find dynamic queries influencing Database.Query calls that may evaluate to a null.
CxList dbQuery = All.FindByMemberAccess("database.query");
CxList nulls = All.FindByType(typeof(NullLiteral));
nulls = nulls.FindByRegex("null");

//Find static SOQL statements with a variable that may evaluate to a null.
CxList strings = Find_Strings();
CxList soqlParams = All.GetParameters(All.FindByMemberAccess("Cx_VirtualDal.select"));
CxList soqlStrings = strings.GetByAncs(soqlParams);
CxList soqlVars = All.FindByType(typeof(UnknownReference)).GetByAncs(soqlParams);

System.Collections.Generic.List<string> whereParameters = new System.Collections.Generic.List<string>();
System.Text.RegularExpressions.Regex variablePattern = new System.Text.RegularExpressions.Regex(@":\s*(\w+)");
System.Text.RegularExpressions.Match match;

CxList inVariables = All.NewCxList();
foreach (CxList curSOQL in soqlStrings)
{	
	whereParameters = curSOQL.ExtractFromSOQL("where");
	
	foreach(string inWhere in whereParameters) 
	{
		match = variablePattern.Match(inWhere);//Find matches in "where" that are variables.
		while (match.Success)
		{
			inVariables.Add(soqlVars.FindByShortName(match.Groups[1].Value));
			match = match.NextMatch();
		}
	}
}
 
result = (dbQuery + inVariables).DataInfluencedBy(nulls);