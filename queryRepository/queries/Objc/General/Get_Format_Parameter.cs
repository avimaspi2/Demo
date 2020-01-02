CxList methods = Find_Methods();

CxList func1 = methods.FindByShortNames(new List<string> {"scanf",
		"printf",
		"printf:",
		"vprintf",
		"NSLog" ,
		"NSLogv"});

func1.Add(All.FindByMemberAccess("CString.Format"));
func1.Add(All.FindByMemberAccess("CString.FormatV"));
func1.Add(All.FindByMemberAccess("CString.FormatMessage"));
func1.Add(All.FindByMemberAccess("CStringT.Format"));
func1.Add(All.FindByMemberAccess("CStringT.FormatV"));
func1.Add(All.FindByMemberAccess("CStringT.FormatMessage"));
func1.Add(All.FindByMemberAccess("CStringT.FormatMessageV"));

CxList firstParam = All.GetParameters(func1, 0);

CxList func2 = methods.FindByShortNames(new List<string> {"sscanf",
		"vsprintf",
		"asprintf",
		"vasprintf",
		"fprintf",
		"sprintf",
		"syslog"});

func2.Add(methods.FindByMemberAccess("NSException.raise:*format:*"));

CxList secondParam = All.GetParameters(func2, 1);

CxList func3 = Find_All_snprintf();

List<string> func3MethodsNames = new List<string> {
		"CFStringAppendFormatAndArguments","CFStringAppendFormat",
		"CFStringCreateWithFormatAndArguments","CFStringCreateWithFormat"
		};

func3.Add(methods.FindByShortNames(func3MethodsNames));

CxList thirdParam = All.GetParameters(func3, 2);

CxList parameters = All.NewCxList();
parameters.Add(firstParam);
parameters.Add(secondParam);
parameters.Add(thirdParam);

parameters -= parameters.FindByType(typeof(Param));

// Concatenate the parameters with their methods
CxList funcs = All.NewCxList();
funcs.Add(func1);
funcs.Add(func2);
funcs.Add(func3);

foreach (CxList p in parameters)
{
	CxList f = funcs.FindByParameters(p);
	result.Add(p.ConcatenateAllPaths(f));
}

// Concatenate the parameters with their methods
List<string> stringWithFormatMethodsNames = new List<string> {
		"initWithFormat:*","localizedStringWithFormat:*",
		"stringByAppendingFormat*","appendingFormat*",
		"stringWithFormat:*","appendFormat:*","printf","print:",
		// Swift
		// init(format:locale:arguments:)
		// init(format:arguments:)
		"NSString:","NSString:locale:arguments","NSString:arguments"
		};

CxList stringWithFormat = methods.FindByShortNames(stringWithFormatMethodsNames);

CxList formatSecondParameter = methods.FindByName("*sscanf*");
formatSecondParameter.Add(methods.FindByName("*syslog*"));

parameters = All.GetParameters(stringWithFormat, 0);
parameters.Add(All.GetParameters(formatSecondParameter, 1));
parameters -= parameters.FindByType(typeof(Param));

CxList formatMethods = All.NewCxList();
formatMethods.Add(stringWithFormat);
formatMethods.Add(formatSecondParameter);


foreach (CxList swf in formatMethods)
{
	CxList p = parameters.GetParameters(swf);
	result.Add(p.ConcatenateAllPaths(swf));
}