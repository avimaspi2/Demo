CxList methods = Find_Methods();

string[] sys = new string[] {"stdin", "argv"};
CxList sysMethods = Find_Methods_By_Import("sys", sys);

CxList stdinRead = sysMethods.GetMembersOfTarget().FindByShortName("read");

CxList argv = sysMethods.FindByShortName("argv");
CxList parseArgs = methods.FindByShortName("parse_args");

string[] input = new string[] {"input"};
CxList fileInput = Find_Methods_By_Import("file_input", input);
result = methods.FindByShortNames(new List<string>{"input", "raw_input", "readline", "readlines"});

CxList msvcrtMemberAccess = methods.FindByMemberAccess("msvcrt.*");
result.Add(msvcrtMemberAccess.FindByShortNames(new List<string>{"getch", "getwch", "getche", "getwche"}));

result.Add(All.FindByName("os.environ.*"));
result.Add(stdinRead);
result.Add(argv);
result.Add(fileInput);

CxList parseArgsParams = All.GetParameters(parseArgs);

foreach (CxList pa in parseArgs)
{
	CxList paParams = parseArgsParams.GetParameters(pa);
	if (paParams.Count == 0)
	{
		result.Add(pa);
	}
}