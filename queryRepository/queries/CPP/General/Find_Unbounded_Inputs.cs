// Find c string that are used as buffer for input, and their size is unknown
CxList methodDecl = Find_MethodDecls();
CxList methodInvoke = Find_Methods();

// main second param
List <string> mains = new List<string> {"main", "Main", "_main", "_tmain", "Winmain", "AfxWinMain"};
CxList main = methodDecl.FindByShortNames(mains, false);
CxList mainSecParam = All.GetParameters(main, 1);

// gets first param
CxList firstParamMethods = methodInvoke.FindByShortNames(new List<string> {"gets"});
CxList firstParam = All.GetParameters(firstParamMethods, 0);

// Methods which their second or higher param are used as string buffer, like: scanf("%s%d%s", str1, i, str2);
CxList secondAndUpParamMethods = methodInvoke.FindByShortNames(new List<string> {"scanf", "vscanf"});
CxList secondAndUpParams = All.GetParameters(secondAndUpParamMethods)
	- All.GetParameters(secondAndUpParamMethods, 0);

// Methods which their third or higher param are used as string buffer, like: fscanf(fp, "%s%d%s", str1, i, str2);
List <string> f_s_scanf = new List<string>{"fscanf", "vfscanf", "sscanf", "vsscanf"};
CxList thirdAndUpParamMethods = methodInvoke.FindByShortNames(f_s_scanf);
CxList thirdAndUpParams = All.GetParameters(thirdAndUpParamMethods) 
	- All.GetParameters(thirdAndUpParamMethods, 0)
	- All.GetParameters(thirdAndUpParamMethods, 1);

// catgets fourth param
CxList fourthParamMethods = methodInvoke.FindByShortNames(new List<string> {"catgets"});
CxList fourthParam = All.GetParameters(fourthParamMethods, 3);

// All the relevant params
CxList relevantParams = firstParam;
relevantParams.Add(secondAndUpParams);
relevantParams.Add(thirdAndUpParams);
relevantParams.Add(fourthParam);
relevantParams.Add(mainSecParam);

// The return value of methods
CxList envMethods = methodInvoke.FindByShortNames(new List<string> {"getenv", "_wgetenv", "getpass"});

CxList shortNames = All.FindByShortName("m_lpCmdLine");

// Find all buffers influenced by std::cin
CxList cin = All.FindByShortNames(new List<string> {"cin", "wcin"});
CxList cinBuff = All.DataInfluencedBy(cin).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
cinBuff = cinBuff.FindByType(typeof(UnknownReference));

// Find all istream::get methods
// The only overloading get method that is unbounded (doesn't get a size parameter) are the following overloads:
// istream& get(streambuf& sb);
// istream& get(streambuf & sb, char delim);
CxList inGet = methodInvoke.FindByMemberAccess("istream.get");
inGet.Add(methodInvoke.FindByMemberAccess("fstream.get"));
inGet.Add(methodInvoke.FindByMemberAccess("ifstream.get"));

CxList inGetParams = All.GetByAncs(All.GetParameters(inGet, 0));
CxList inGetVulnParams = inGetParams.FindByType("streambuf");
inGetVulnParams.Add(inGetParams.FindByType("std.streambuf"));

result = relevantParams;
result.Add(shortNames);
result.Add(cinBuff);
result.Add(inGetVulnParams);
result.Add(envMethods);