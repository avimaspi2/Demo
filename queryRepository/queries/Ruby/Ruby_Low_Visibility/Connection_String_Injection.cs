CxList dbDef = 
	All.FindByName("*Mongo.Connection") + 
	All.FindByName("*Driver.Mongo");
CxList http = dbDef.GetMembersOfTarget().FindByShortName("from_uri");
http = All.GetByAncs(All.GetParameters(http));
CxList input = Find_Interactive_Inputs();
CxList sanitize = Find_Integers();

result = http.InfluencedByAndNotSanitized(input, sanitize);