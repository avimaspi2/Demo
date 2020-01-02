// The correct sanitization shoud be added

CxList malloc = Find_Methods().FindByShortName("malloc");

// Find input influence on size of copy
CxList sizeParam = All.GetByAncs(malloc) - malloc;
CxList db = Find_DB() + Find_Read();
CxList sanitize = Find_All_Strlen();

result = sizeParam.InfluencedByAndNotSanitized(db, sanitize);