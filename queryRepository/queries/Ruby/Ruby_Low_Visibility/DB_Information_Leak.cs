CxList dbAll = Find_MongoDB_Def().GetMembersOfTarget();
CxList dbInfo = dbAll.FindByShortName("database_*") + dbAll.FindByShortName("collection_*");
CxList outputs = Find_Interactive_Outputs() + Find_Console_Inputs();
CxList sanitize = Find_Integers();

result = dbInfo.InfluencingOnAndNotSanitized(outputs, sanitize);