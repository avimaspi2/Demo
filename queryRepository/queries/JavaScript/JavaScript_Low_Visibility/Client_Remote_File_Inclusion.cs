CxList methodInvokes = Find_Methods();

CxList stringLiterals = Find_String_Literal();

CxList scriptTag = Find_MethodDecls().FindByShortName("script__*");
CxList src = methodInvokes.FindByShortName("src").GetByAncs(scriptTag);
CxList parameter = stringLiterals.GetParameters(src);

//Add SAP includeScript
CxList sapLibraries = Find_SAP_Library();
CxList includeScript = sapLibraries.GetMembersOfTarget().FindByShortName("includeScript");
CxList scriptURLParameter = stringLiterals.GetParameters(includeScript, 0);

parameter.Add(scriptURLParameter);

foreach(CxList prm in parameter)
{
	CSharpGraph url = prm.GetFirstGraph();
	if(url != null && url.ShortName != null)
	{
		string urlString = url.ShortName;
		urlString = urlString.TrimStart(new char []{'\'','\"'});
		urlString = urlString.TrimEnd(new char []{'\'','\"'});
		if((urlString.StartsWith("http") || urlString.StartsWith("https") || urlString.StartsWith("//") || urlString.StartsWith("www")) && 
			!urlString.ToLower().Contains("jquery") && 
			!urlString.ToLower().Contains("angular"))
		{
			result.Add(prm);
		}
	}
}

// require("http://...)
// import ... from "http://..."

CxList requireURIs = All.NewCxList();
requireURIs.Add(Find_Require("http://*"));
requireURIs.Add(Find_Require("https://*"));
requireURIs.Add(Find_Require("//*"));
requireURIs = requireURIs.FindByType(typeof(Import));

List<string> safeDomains = new List<string> {
		"cdnjs.cloudflare.com",
		"code.jquery.com",
		"ajax.googleapis.com",
		"cdn.jsdelivr.net",
		"maxcdn.bootstrapcdn.com",
		};

List<string> stringURIsSafe = new List<string>();
foreach (string s in safeDomains) 
{
	stringURIsSafe.Add("http://" + s);
	stringURIsSafe.Add("https://" + s);
	stringURIsSafe.Add("//" + s);
}

CxList requireURIsSafe = All.NewCxList();

foreach(CxList imp in requireURIs){
	
	Import import = imp.TryGetCSharpGraph<Import>();
	if (import != null)
	{
		foreach(string safeUri in stringURIsSafe){
			if(import.ImportedFilename.StartsWith(safeUri)){
				requireURIsSafe.Add(imp);
				break;
			}
		}	
	}
}

result.Add(requireURIs - requireURIsSafe);