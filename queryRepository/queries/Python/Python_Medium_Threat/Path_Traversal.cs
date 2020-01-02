CxList inputs = Find_Interactive_Inputs();

CxList methods = Find_Methods();
CxList obj = Find_Declarators();
obj.Add(methods);

//File access
CxList files = obj.FindByName("open");
files.Add(obj.FindByName("file"));
files.Add(Find_Methods_By_Import("io", new string[]{"FileIO", "open", "BufferedReader", "BufferedRandom",
	"BufferedRWPair", "TextIOBase", "TextIOWrapper"}));
files.Add(Find_Methods_By_Import("codecs", new string[]{"StreamReader", "StreamReaderWriter", "StreamRecoder"}));
files.Add(Find_Methods_By_Import("fileinput", new string[]{"input", "FileInput"}));
files.Add(Find_Methods_By_Import("linecache", new string[]{"getline"}));

//Directory access
files.Add(Find_Methods_By_Import("os.path", new string[]{"join", "walk"}));
files.Add(Find_Methods_By_Import("macpath", new string[]{"walks"}));
files.Add(Find_Methods_By_Import("dircache", new string[]{"listdir", "opendir"}));
files.Add(Find_Methods_By_Import("glob", new string[]{"glob", "iglob"}));
files.Add(Find_Methods_By_Import("fnmatch", new string[]{"filter"}));

CxList filesMethodsAll = files.GetMembersOfTarget();

CxList filesMethods = filesMethodsAll.FindByShortName("Close");
filesMethods.Add(filesMethodsAll.FindByShortName("Dispose"));

files -= filesMethods.GetTargetOfMembers();

CxList sanitized = Find_Integers();
sanitized.Add(Find_Methods_By_Import("os.path", new string[]{"abspath", "basename", "commonprefix",
	"expanduser", "expandvars", "realpath", "relpath"}));

//Formatting strings with % and format() should be considered sanitizers
CxList format = Find_BinaryExpr().FindByRegex(@"%");
sanitized.Add(All.GetByAncs(format).FindByType(typeof(UnknownReference)));
sanitized.Add(methods.FindByShortName("format"));

result = files.InfluencedByAndNotSanitized(inputs, sanitized).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);