CxList requests = All.NewCxList();
requests.Add(All.FindByMemberAccess("HttpServletRequest.getQueryString"));
requests.Add(All.FindByName("*request.getQueryString"));
requests.Add(All.FindByName("*Request.getQueryString"));

result = requests;