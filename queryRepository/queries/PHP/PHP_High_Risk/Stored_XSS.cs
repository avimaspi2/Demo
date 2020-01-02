CxList db = Find_DB_Out() + Find_Read();
CxList resFetch = Find_Methods().FindByShortName("*fetch*", false);
resFetch -= resFetch.FindByShortNames(new List<String>(){ "*fetch_feed*", "*fetchmode*" }, false);
db.Add(resFetch);
db -= db.FindByShortName("*rss*", false);

CxList outputs = Find_Interactive_Outputs();
CxList sanitize = Find_XSS_Sanitize();
sanitize.Add(outputs * Find_ContentType_XSS_Sanitizers());

result = db.InfluencingOnAndNotSanitized(outputs, sanitize) + Find_cURL_XSS();