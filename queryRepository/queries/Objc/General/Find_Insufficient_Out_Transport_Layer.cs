CxList requests = All.FindByMemberAccess("NSMutableURLRequest.*");
requests.Add(All.FindByMemberAccess("NSURLRequest.*"));
requests.Add(All.FindByMemberAccess("URLRequest.*"));

CxList pureHTTP = Find_Pure_Http();

CxList outputs = requests
	.DataInfluencedBy(pureHTTP)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly)
	.GetTargetOfMembers();
outputs = All.FindAllReferences(outputs).GetMembersOfTarget();

List<string> methodsAll = new List<string> {
		"addValue:forHTTPHeaderField:",
		"dataTaskWithURL:*" ,
		"downloadTaskWithURL:*",
		"initWithURL:",
		"initWithURL:cachePolicy:timeoutInterval:",
		"requestWithURL:",		
		"requestWithURL:cachePolicy:timeoutInterval:",
		"setAllHTTPHeaderFields:" ,
		"setHTTPBody:",
		"setHTTPBodyStream:",
		"setMainDocumentURL:",
		"setURL:",		
		"setValue:forHTTPHeaderField:",
		// The following are syntactic sugar of the above setters:		
		"allHTTPHeaderFields" ,
		"HTTPBody",
		"HTTPBodyStream",
		"mainDocumentURL",
		"URL",		
		"URLRequest:url:" ,
		"URLRequest:url:cachePolicy:timeoutInterval:",
		"URLRequest:cachePolicy:timeoutInterval:"		
		};

CxList firstParameterOutputs = outputs.FindByShortNames(methodsAll);
firstParameterOutputs.Add(outputs.FindByShortName("URLRequest*").FindByParameterName("url"));

CxList secondParameterOutputs = outputs.FindByShortName("uploadTaskWithRequest:fromData:*");
firstParameterOutputs.Add(outputs.FindByShortName("uploadTask:from:"));
firstParameterOutputs.Add(outputs.FindByShortName("uploadTask:from:*").FindByParameterName("with"));


CxList allParametersOutputs = All.NewCxList();
allParametersOutputs.Add(firstParameterOutputs);
allParametersOutputs.Add(secondParameterOutputs);

CxList newOutputs = allParametersOutputs.FindByType(typeof(MemberAccess));

foreach (CxList output in firstParameterOutputs)
{
	newOutputs.Add(All.GetParameters(output, 0).ConcatenateAllPaths(output));
}
foreach (CxList output in secondParameterOutputs)
{
	newOutputs.Add(All.GetParameters(output, 1).ConcatenateAllPaths(output));
}
result = newOutputs;