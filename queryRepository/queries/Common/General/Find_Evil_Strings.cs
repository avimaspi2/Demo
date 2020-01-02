// Find evil strings
string maybe = @"\s*[*?]\??\s*";
string numRep = @"\{\d(,\d)?(\d)+\}";
string rep = @"(\*\??|\+\??|" + numRep + @")\s*";
string brack = @"\[([^\\\]\[]|(\\.))+\]";
brack = @"(" + brack + @"|" + @"\(" + brack + @"\))";
string paren = @"\([^()]+\)";
paren = @"(" + paren + @"|" + @"\(" + paren + @"\))";
string component = @"([\w.,:;@#$%&-+=~/!]|" + brack + @"|\s|\\\s|\\\\\s|" + paren + @"|\\\w|\\\\\w)";
string letter = @"[\-0-9a-zA-Z_]";
component = @"(" + component + @"|" + @"\(" + component + @"\))";
String[] evilStringsList1 = { // Wrapped with (...)*
	@"[^()\.]*\." + rep,
	component + rep,
	@"\." + component + rep,
	component + rep + @"\.",
	@"(.+)(" + rep + @"|\?)*(" + component + maybe + @")*\|(" + component + @"[?*]|\2[?*+]?)*(\2[?*+]?)+(" + component + @"[?*]|\2[?*+]?)*",
	@"\[[^\]\\w]*\\\\w\]" + rep + @"\[" + letter + @"+\]",
	@"\.\*[^()+*\\]+",
	@"\[[^\\\[\]]*((\\)?[^\\])[^\[\]]*\][+*?]\2",
	@"\[[^\\\[\]]*((\\\\)?[^\\])[^\[\]]*\][+*?]\2",
	@"(" + component + maybe + @")*" + @"(" + component + rep + @")(" + component + maybe + @")*",
	@"(" + component + maybe + @")*" + @"\((" + component + rep + @"\))(" + component + maybe + @")*",
	@"\[([^\\\[\]]|(\\[^w]))*(\\)?\\w[^\[\]]*\]\[[^\w\[\]]*\w+[^\[\]]*\]" + rep,
	@"\[([^\\\[\]]|(\\[^w]))*(\\)?\\w[^\[\]]*\]" + rep + @"\[[^\w\[\]]*\w+[^\[\]]*\]",
	@"\[[^\w\[\]]*\w+[^\[\]]*\]\[([^\\\[\]]|(\\[^w]))*(\\)?\\w[^\[\]]*\]" + rep,
	@"\[[^\w\[\]]*\w+[^\[\]]*\]" + rep + @"\[([^\\\[\]]|(\\[^w]))*(\\)?\\w[^\[\]]*\]",
	@"((\\)?.)\[\2[^\[\]]*\]" + rep,
	@"((\\)?.)\[[^\[\]\^][^\[\]]*\2[^\[\]]*\]" + rep,
	@"((\\)?.)\(\[\2[^\[\]]*\]" + rep + @"\)",
	@"((\\)?.)\(\[[^\[\]\^][^\[\]]*\2[^\[\]]*\]" + rep + @"\)",
	@"[^|]+\|" + component + rep,
	component + rep + @"\|[^)]*",
	component + maybe + component + maybe
	};

String[] evilStringsList2 = { // Not wrapped with (...)*
	@"(" + component + @")" + rep + @"(" + component + maybe + @")?\2" + rep,
	@"(^|[^\\])" + letter + rep + @"(" + component + maybe + @")?\\w" + rep,
	@"\[" + letter + @"+\]" + rep + @"(" + component + maybe + @")?\\w" + rep,
	@"\\w" + rep + @"(" + component + maybe + @")?" + letter + rep,
	@"\\w" + rep + @"(" + component + maybe + @")?\[" + letter + @"+\]" + rep,
	};

string preEvil = @"\((\?\:)?";
string postEvil = @"\)" + rep;

CxList strings = Find_Strings();

foreach (string evilString in evilStringsList1)
{
	System.Text.RegularExpressions.Regex evil = new System.Text.RegularExpressions.Regex(preEvil + evilString + postEvil);

	foreach (CxList suspectedString in strings)
	{
		CSharpGraph gr = suspectedString.TryGetCSharpGraph<CSharpGraph>();
		if(gr == null || gr.ShortName == null)
		{
			continue;
		}
		if (gr.ShortName.Length < 1000)
		{
			if (evil.Match(gr.ShortName).Success)
			{
				result.Add(suspectedString);
			}
		}
	}
}

foreach (string evilString in evilStringsList2)
{
	System.Text.RegularExpressions.Regex evil = new System.Text.RegularExpressions.Regex(evilString);

	foreach (CxList suspectedString in strings)
	{
		CSharpGraph gr = suspectedString.TryGetCSharpGraph<CSharpGraph>();
		if(gr == null || gr.ShortName == null)
		{
			continue;
		}
		if (gr.ShortName.Length < 1000)
		{
			if (evil.Match(gr.ShortName).Success)
			{
				result.Add(suspectedString);
			}
		}
	}
}

result -= result.FindByShortNames(new List<String>(){ "*++*", "****", "void*"});