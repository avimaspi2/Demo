CxList methods = Find_Methods();
CxList inputs = Find_Interactive_Inputs() + Find_Read();

CxList download = methods.FindByShortName("send_file");
download.Add(methods.FindByShortName("send_data"));
CxList downloadedData = All.GetParameters(download, 0);

CxList sanitize = Find_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(downloadedData, sanitize);
result.Add((inputs * downloadedData) - sanitize);