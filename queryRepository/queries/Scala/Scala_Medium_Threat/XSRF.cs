CxList requests = All.NewCxList();
requests.Add(Find_Interactive_Inputs());
requests.Add(All.FindByMemberAccess("HttpServletRequest.getQueryString"));
requests.Add(All.FindByName("*request.getQueryString"));
requests.Add(All.FindByName("*Request.getQueryString"));

CxList java_xsrf_sanitize = Find_XSRF_Sanitize();
requests -= requests.GetByAncs(java_xsrf_sanitize);

CxList db = Find_DB_In();
CxList strings = Find_Strings();

CxList write = All.NewCxList();
write.Add(strings.FindByName("*update*", false));
write.Add(strings.FindByName("*delete*", false));
write.Add(strings.FindByName("*insert*", false));

CxList dbWrite = db.DataInfluencedBy(write);
dbWrite.Add(db.FindByShortNames(new List<string> {"update*", "delete*","insert*"}));

result = dbWrite.DataInfluencedBy(requests);