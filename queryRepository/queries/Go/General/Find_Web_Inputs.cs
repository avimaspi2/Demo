// Query to find inputs that comes from HTTP requests
CxList variables = Find_UnknownReferences();
CxList requests = variables.FindByPointerType("http.Request");

List<string> requestMemberNames = new List<string>(){
	"Body", "Cookie*", "Form*", "GetBody", "Header", "Host", "Multipart*", "Method",
	"PostForm*", "Proto", "Query", "RemoteAddr", "RequestURI", "Response", "Trailer", 
	"URL", "UserAgent"
};

result.Add(requests.GetMembersOfTarget().FindByShortNames(requestMemberNames));