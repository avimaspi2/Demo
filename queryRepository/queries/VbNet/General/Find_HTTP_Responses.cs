result = All.FindByTypes(new String[]{"HttpResponse",
	"HttpResponseMessage",
	"HttpListenerResponse","HttpResponseBase","HttpResponseWrapper","HttpWebResponse"}, false);

result.Add(All.FindByTypes(new String[]{"HttpContext",
	"HttpListenerContext"}, false).GetMembersOfTarget().FindByShortName("Response", false));