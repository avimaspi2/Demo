/* This query returns methods that change HTTP responses's headers */
CxList httpClasses = All.FindByTypes(new String[]{"HttpPost", "HttpMessage", "HttpResponse",
	"BasicHttpResponse", "HttpServletResponseWrapper", "HttpServletResponse",
	"ServletResponse", "CloseableHttpResponse"});
CxList httpHeaderMethods = httpClasses.GetMembersOfTarget().FindByShortNames(new List<String>{"addHeader", "setHeader", "setHeaders"});

CxList containerResponseContext = All.FindByType("ContainerResponseContext");
CxList containerHeaders = containerResponseContext.GetMembersOfTarget().FindByShortName("getHeaders");
CxList headers = All.FindAllReferences(containerHeaders.GetAssignee());

headers = headers.GetMembersOfTarget();

CxList headersMethodsSimple = headers.FindByShortNames(
	new List<string>(){"add","putSingle","addFirst","putIfAbsent","put","replace","addAll"});

result.Add(httpHeaderMethods);
result.Add(headersMethodsSimple);