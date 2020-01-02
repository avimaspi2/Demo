// Find Pure Http and its downloaded Data
CxList http = Find_Pure_Http();
CxList httpDownloadedData = Find_Downloaded_Data(http);

result = http;
result.Add(httpDownloadedData);