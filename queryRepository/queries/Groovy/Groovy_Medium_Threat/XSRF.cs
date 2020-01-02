CxList requests = 
	Find_Interactive_Inputs() + 
	All.FindByMemberAccess("HttpServletRequest.getQueryString") +
	All.FindByName("*request.getQueryString") +  
	All.FindByName("*Request.getQueryString");
CxList db = Find_DB_In();
CxList strings = Find_Strings();

CxList write = strings.FindByName("*update*", false) +
	strings.FindByName("*delete*", false) +
	strings.FindByName("*insert*", false);

CxList dbWrite = db.DataInfluencedBy(write);
dbWrite.Add(db.FindByShortName("update*"));
dbWrite.Add(db.FindByShortName("delete*"));
dbWrite.Add(db.FindByShortName("insert*"));

result = dbWrite.DataInfluencedBy(requests);