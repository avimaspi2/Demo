// Information_Exposure_Through_Query_String
// ////////////////////////////////////////-
// The purpose of the query is as to find applications that allow the following:
//		Use the http string for sending protected data.
CxList methods = Find_Methods();

CxList outputMethods = All.FindByMemberAccess("NSMutableURLRequest.initWithURL:");
outputMethods.Add(All.FindByMemberAccess("NSMutableURLRequest.requestWithURL:"));
outputMethods.Add(All.FindByMemberAccess("NSMutableURLRequest.requestWithURL:cachePolicy:timeoutInterval:")); 
outputMethods.Add(All.FindByMemberAccess("NSMutableURLRequest.setURL:"));
	
outputMethods.Add(All.FindByMemberAccess("NSURLRequest.initWithURL:"));
outputMethods.Add(All.FindByMemberAccess("NSURLRequest.initWithURL:cachePolicy:timeoutInterval:"));
outputMethods.Add(All.FindByMemberAccess("NSURLRequest.requestWithURL:")); 
outputMethods.Add(All.FindByMemberAccess("NSURLRequest.requestWithURL:cachePolicy:timeoutInterval:"));

outputMethods.Add(methods.FindByShortName("NSURLRequest:").FindByParameterName("url"));
outputMethods.Add(methods.FindByShortName("URLRequest:").FindByParameterName("url"));
outputMethods.Add(All.FindByMemberAccess("URLRequest:url:"));
outputMethods.Add(All.FindByMemberAccess("URLRequest:url:cachePolicy:timeoutInterval:"));
	
outputMethods.Add(Find_By_Constructor_Name("NSMutableURLRequest.initWithURL:"));
outputMethods.Add(Find_By_Constructor_Name("NSMutableURLRequest.initWithURL:cachePolicy:timeoutInterval:"));

outputMethods.Add(Find_By_Constructor_Name("NSMutableURLRequest.initWithURL:"));
outputMethods.Add(Find_By_Constructor_Name("NSMutableURLRequest.initWithURL:cachePolicy:timeoutInterval:"));
	
outputMethods.Add(Find_By_Constructor_Name("NSURLRequest.initWithURL:"));
outputMethods.Add(Find_By_Constructor_Name("NSURLRequest.initWithURL:cachePolicy:timeoutInterval:"));

outputMethods.Add(Find_By_Constructor_Name("URLRequest.init:"));
outputMethods.Add(Find_By_Constructor_Name("URLRequest.init:url:"));
outputMethods.Add(Find_By_Constructor_Name("URLRequest.init:url:cachePolicy:timeoutInterval:"));
outputMethods.Add(Find_By_Constructor_Name("URLRequest.init:cachePolicy:timeoutInterval:"));

outputMethods.Add(methods.FindByMemberAccess("UIWebView.loadRequest:"));

CxList personalInfo = Find_Personal_Info();

CxList outputParams = All.GetParameters(outputMethods, 0);

result = outputParams.DataInfluencedBy(personalInfo);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);