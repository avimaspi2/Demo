CxList sid = 	All.FindByName("*Request.QueryString_SID*", false) + 
				All.FindByName("*Request.QueryString_Session*", false);

CxList queryString = 	All.FindByMemberAccess("HttpRequest.QueryString_*") +
						All.FindByName("*Request.QueryString_*");

result = sid.GetParameters(queryString);