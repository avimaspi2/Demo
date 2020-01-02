CxList db = Find_DB();
CxList outputs = Find_XSS_Outputs();
outputs.Add(Find_XSS_Outputs_Webview());

CxList sanitize = Find_XSS_Sanitize();
CxList reads = Find_Read();

CxList dbRead = All.NewCxList();
dbRead.Add(db);	
dbRead.Add(reads);

result = dbRead.InfluencingOnAndNotSanitized(outputs, sanitize);