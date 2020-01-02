CxList sid = 	All.FindByName("*request.querystring_sid*", false) + 
				All.FindByName("*request.querystring_session*", false);

CxList queryString = 	All.FindByMemberAccess("httprequest.Querystring_*") +
						All.FindByName("*request.QueryString_*");

result = sid.GetParameters(queryString);