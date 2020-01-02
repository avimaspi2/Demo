CxList sid = All.FindByShortName("\"Session*", false);

CxList queryString = All.FindByMemberAccess("ServletRequest.getParameter");

result = sid.GetParameters(queryString);