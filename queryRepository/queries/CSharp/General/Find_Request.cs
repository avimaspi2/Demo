CxList request_binaryRead = All.FindByMemberAccess("Request.BinaryRead");
CxList request_cookies = All.FindByMemberAccess("Request.Cookies*");
CxList request_form = All.FindByMemberAccess("Request.Form");
CxList request_headers = All.FindByMemberAccess("Request.Headers");
CxList request_item = All.FindByMemberAccess("Request.Item");
CxList request_param = All.FindByMemberAccess("Request.Params");
CxList request_querystring = All.FindByMemberAccess("Request.QueryString*");
CxList request_rawurl = All.FindByMemberAccess("Request.RawUrl");
CxList request_url = All.FindByMemberAccess("Request.Url");
CxList request_urlreferrer = All.FindByMemberAccess("Request.UrlReferrer");
CxList request_userlanguages = All.FindByMemberAccess("Request.UserLanguages");

CxList inputs_request = All.FindByMemberAccess("HttpRequest.*");

inputs_request.Add(request_binaryRead);
inputs_request.Add(request_binaryRead.GetMembersOfTarget());
inputs_request.Add(request_cookies);
inputs_request.Add(request_cookies.GetMembersOfTarget());
inputs_request.Add(request_form);
inputs_request.Add(request_form.GetMembersOfTarget());
inputs_request.Add(request_headers);
inputs_request.Add(request_headers.GetMembersOfTarget());
inputs_request.Add(request_item);
inputs_request.Add(request_item.GetMembersOfTarget());
inputs_request.Add(request_param);
inputs_request.Add(request_param.GetMembersOfTarget());
inputs_request.Add(request_querystring);
inputs_request.Add(request_querystring.GetMembersOfTarget());
inputs_request.Add(request_rawurl);
inputs_request.Add(request_rawurl.GetMembersOfTarget());
inputs_request.Add(request_url);
inputs_request.Add(request_url.GetMembersOfTarget());
inputs_request.Add(request_urlreferrer);
inputs_request.Add(request_urlreferrer.GetMembersOfTarget());
inputs_request.Add(request_userlanguages);
inputs_request.Add(request_userlanguages.GetMembersOfTarget());
inputs_request.Add(All.FindByName("Page.Request.ClientCertificate"));
inputs_request.Add(All.FindByName("Page.Request.Cookies*"));
inputs_request.Add(All.FindByName("Page.Request.Form"));
inputs_request.Add(All.FindByName("Page.Request.Headers"));
inputs_request.Add(All.FindByName("Page.Request.Params"));
inputs_request.Add(All.FindByName("Page.Request.QueryString*"));
inputs_request.Add(All.FindByName("Page.Request.RawUrl"));
inputs_request.Add(All.FindByName("Page.Request.Url"));
inputs_request.Add(All.FindByName("Page.Request.UrlReferrer"));
inputs_request.Add(All.FindByName("Request"));
inputs_request.Add(All.FindByName("Page.Request")); 
inputs_request.Add(All.FindByName("Request.ClientCertificate"));
inputs_request.Add(All.FindByName("Request.Cookies*"));
inputs_request.Add(All.FindByName("Request.Form"));
inputs_request.Add(All.FindByName("Request.Headers"));
inputs_request.Add(All.FindByName("Request.Params"));
inputs_request.Add(All.FindByName("Request.QueryString*"));
inputs_request.Add(All.FindByName("Request.RawUrl"));
inputs_request.Add(All.FindByName("Request.Url"));
inputs_request.Add(All.FindByName("Request.UrlReferrer"));
inputs_request.Add(All.FindByName("Request.Query"));

CxList httpRequest = All.FindByType("HttpRequest");
inputs_request.Add(httpRequest.GetByAncs(httpRequest.FindByType(typeof(IndexerRef))).FindByType(typeof(UnknownReference)));

result = inputs_request;

// WebListener for .Net Core
CxList context = All.FindByMemberAccess("WebListener.AcceptAsync");
CxList assigned = context.GetAssignee();
context.Add(All.FindAllReferences(assigned));

CxList contextMembers = context.GetMembersOfTarget();
CxList request = contextMembers.FindByShortName("Request");
CxList requestRef = request.GetAssignee();
request.Add(All.FindAllReferences(requestRef));

CxList tragets = request.GetMembersOfTarget();
List <string> membersList = new List<string> (){"Body", "ContentType", "Headers", "Path", "PathBase", "RawUrl", "RemoteIpAddress", "RemotePort"};
CxList requestMembers = tragets.FindByShortNames(membersList);
result.Add(requestMembers);

result.Add(All.FindByMemberAccess("Request.Body"));
result.Add(All.FindByMemberAccess("Request.ContentType"));
result.Add(All.FindByMemberAccess("Request.Path"));
result.Add(All.FindByMemberAccess("Request.PathBase"));
result.Add(All.FindByMemberAccess("Request.RemoteIpAddress"));
result.Add(All.FindByMemberAccess("Request.RemotePort"));