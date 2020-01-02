// Find the string literals containig "password"
CxList psw = Find_Password_Strings();
CxList emptyString = Find_Empty_Strings();
CxList nullLiteral = All.FindByShortName("none", false);

CxList emptyStringNull = All.NewCxList();
emptyStringNull.Add(emptyString);
emptyStringNull.Add(nullLiteral);

CxList strLiterals = Find_Strings() - emptyStringNull;

// Find creation of connections or connection strings, influenced by password
CxList methods = Find_Methods();
CxList connection = methods.FindByShortName("*connect*", false);
connection.Add(Find_DB_Conn_Strings());

result = connection.DataInfluencedBy(psw);

// Find connection strings that contain a password in their initialization
CxList connetionParam2 = All.GetParameters(connection, 2);
CxList connetionParam1 = All.GetParameters(connection, 1);

CxList ancsPsw = All.GetByAncs(All.GetParameters(connection)
	.FindByType(typeof(ArrayInitializer))).FindByType(typeof(UnknownReference)) * psw;

CxList connetionParams = All.NewCxList();
connetionParams.Add(connetionParam1);
connetionParams.Add(connetionParam2);

CxList pwdInConnectioParam = strLiterals.GetByAncs(connetionParams * psw);
pwdInConnectioParam.Add(ancsPsw);

// Add the path from the string/parameter to its method
result.Add(connection.DataInfluencedBy(pwdInConnectioParam));

// Add the parameter itself, or whatever is influencing it
CxList paramsAffectedByString = (connetionParam2 * strLiterals);
paramsAffectedByString.Add(connetionParam2.InfluencedBy(strLiterals));
paramsAffectedByString.Add((connetionParam1 * strLiterals));
paramsAffectedByString.Add(connetionParam1.InfluencedBy(strLiterals));

paramsAffectedByString *= psw;

result.Add(connection.DataInfluencedBy(paramsAffectedByString));

// Add connection strings in URL format
string urlRegexStr = "^\"*[a-zA-Z0-9_\\+\\-]+://[a-zA-Z0-9_\\.\\-]+:[a-zA-Z0-9_\\.\\-&amp;%\\$]+@.*$";
System.Text.RegularExpressions.Regex urlRegexCompiled = new System.Text.RegularExpressions.Regex(urlRegexStr);

string urlRegexStr2 = @"(mongodb|amqp)://[^\s]+:[^\s]+@";
System.Text.RegularExpressions.Regex urlRegexCompiled2 = new System.Text.RegularExpressions.Regex(urlRegexStr2);

foreach (CxList suspectedString in strLiterals)
{
	CSharpGraph gr = suspectedString.TryGetCSharpGraph<CSharpGraph>();
	if(gr == null || gr.ShortName == null)
	{
		continue;
	}
	if (gr.ShortName.Length < 1000)
	{
		if (urlRegexCompiled.Match(gr.ShortName).Success || urlRegexCompiled2.Match(gr.ShortName).Success)
		{
			result.Add(suspectedString);
		}
	}
}