// The correct sanitization shoud be added

CxList read = Find_Methods().FindByShortName("read");
read.Add(Find_Methods().FindByShortName("pread"));
read.Add(Find_Methods().FindByShortName("pread64"));

// Find input influence on size of copy
CxList sizeParam = All.GetParameters(read, 2);
sizeParam = All.GetByAncs(sizeParam);
CxList db = Find_Read() + Find_DB();
CxList sanitize = All.GetByAncs(Find_Methods().FindByShortName("sizeof"));

result = sizeParam.InfluencedByAndNotSanitized(db, sanitize);