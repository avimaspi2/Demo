CxList methods = Find_Methods();

CxList func1 = methods.FindByShortNames(new List<string>() {"scanf", "vscanf", "printf", "vprintf"});
func1.Add(All.FindByMemberAccess("CString.Format"));
func1.Add(All.FindByMemberAccess("CString.FormatV"));
func1.Add(All.FindByMemberAccess("CString.FormatMessage"));
func1.Add(All.FindByMemberAccess("CStringT.Format"));
func1.Add(All.FindByMemberAccess("CStringT.FormatV"));
func1.Add(All.FindByMemberAccess("CStringT.FormatMessage"));
func1.Add(All.FindByMemberAccess("CStringT.FormatMessageV"));

CxList firstParam = All.GetParameters(func1, 0);

CxList func2 = methods.FindByShortNames(new List<string>() {
		"sscanf", "vfscanf", 
		"vsprintf", "vsnprintf", 
		"asprintf", "vasprintf",
		"fprintf", "sprintf", "syslog"});

CxList secondParam = All.GetParameters(func2, 1);
CxList func3 = Find_All_snprintf();
CxList thirdParam = All.GetParameters(func3, 2);

CxList parameters = firstParam;
parameters.Add(secondParam);
parameters.Add(thirdParam);

result = parameters;
result.Add(All.GetByAncs(parameters.FindByType(typeof(IndexerRef))).FindByType(typeof(UnknownReference)));