CxList strLiterals = Find_String_Literal();


CxList hardcoded = All.NewCxList();
var domainList = new List<string>{"www.*", "http:*", "https:*"};
hardcoded.Add(strLiterals.FindByShortNames(domainList));


//These methods are the most common in javascript + jquery. 
//They are fundamental to define / construct dynamically valid HTML DOM
CxList commonMethods = All.NewCxList();
var setMethodsList = new List<string>{"*append*", "insertBefore", "insertAdjacentHTML", "prepend"};
CxList setMethods = Find_Methods().FindByShortNames(setMethodsList);
commonMethods.Add(setMethods);

CxList srcFlows = All.FindByType(typeof(MemberAccess)).FindByShortName("*src");
CxList methodsFlows = commonMethods.DataInfluencedBy(hardcoded).IntersectWithNodes(srcFlows, CxList.IntersectionType.AnyNodes);
result.Add(methodsFlows);


// script src=" http... || https... || //... ";
CxList scriptTag = Find_MethodDecls().FindByShortName("script__*");
CxList src = Find_Methods().FindByShortName("src").GetByAncs(scriptTag);
CxList parameter = strLiterals.GetParameters(src);
char[] dels = new char []{'\'','\"'};

foreach(CxList prm in parameter)
{
	CSharpGraph url = prm.GetFirstGraph();
	if(url != null && url.ShortName != null)
	{
		string urlString = url.ShortName.ToLower().Trim(dels);
		if (urlString.StartsWith("http") || urlString.StartsWith("https") || urlString.StartsWith("//"))
		{
			result.Add(prm);
		}
	}
}