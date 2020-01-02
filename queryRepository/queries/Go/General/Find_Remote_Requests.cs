// from https://golang.org/pkg/net/http/ package
// Package url parses URLs and implements query escaping.
CxList netHttpInputs = All.FindByMemberAccess("\"net/http\".*").FindByShortNames(new List<string>{"Get","Head","Post","PostForm"});

CxList httpRequester = All.FindByMemberAccess("\"net/http\".*").FindByShortNames(new List<string>{"Client","Response"});
// eg: catch also &http.Client
httpRequester.Add(httpRequester.GetFathers());

CxList requestOcurrences = All.FindAllReferences(httpRequester.GetAssignee());
List<string> clientMemberNames = new List<string>(){"Do", "Get", "Head", "Post", "PostForm"};
CxList clientMembers = requestOcurrences.GetMembersOfTarget().FindByShortNames(clientMemberNames);
netHttpInputs.Add(clientMembers.ReduceFlowByPragma());

// Grab http.Client and tls.Conn pointers (usually function arguments)
CxList variables = Find_UnknownReferences();
CxList clientType = variables.FindByPointerTypes(new string[] {"http.Client", "tls.Conn"});
CxList allClientTypeMembers = clientType.GetMembersOfTarget();
netHttpInputs.Add(allClientTypeMembers);

// from https://golang.org/pkg/net/url/ package
// Package url parses URLs and implements query escaping.
CxList netUrlInputs = All.FindByMemberAccess("\"net/url\".*").FindByShortNames(new List<string>{"Parse","ParseRequestURI"});

// from https://golang.org/pkg/net/ package
// Package url parses URLs and implements query escaping.
CxList netInputs = All.NewCxList();

List<string> netMembers = new List<string> {
	"SplitHostPort","Lookup*",
	"Dial","DialIP","DialTCP","DialUDP"
	};

netInputs.Add(All.FindByMemberAccess("\"net\".*").FindByShortNames(netMembers));

result.Add(netHttpInputs);
result.Add(netUrlInputs);
result.Add(netInputs);