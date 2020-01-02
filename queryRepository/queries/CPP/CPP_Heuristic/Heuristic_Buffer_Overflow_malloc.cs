// The purpose of the query to protect the system from very big input malloc size
// for example maloc(100000000) can stuck the system

// The correct sanitization shoud be added

CxList malloc = Find_Methods().FindByShortName("malloc");

// Find input influence on size of copy
CxList sizeParam = All.GetByAncs(malloc) - malloc;
CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_All_Strlen();

result = sizeParam.InfluencedByAndNotSanitized(inputs, sanitize);