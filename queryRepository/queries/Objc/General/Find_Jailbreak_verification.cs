CxList methods = Find_Methods();
CxList strings = Find_Strings();

List<string> pathsList = new List<string>() {
		"*/Applications/Cydia.app*",
		"*/Library/MobileSubstrate/MobileSubstrate.dylib*",
		"*/bin/bash*",
		"*/usr/sbin/sshd*",
		"*/etc/apt*",
		"*/private/var/lib/apt/*"};
CxList paths = strings.FindByShortNames(pathsList);
CxList existsCalls = methods.FindByShortName("fileExistsAtPath*");
existsCalls.Add(methods.FindByShortName("fileExists*").FindByParameterName("atPath", 0));

CxList flowsFromPathsToExsists = paths.InfluencingOn(existsCalls);
if (flowsFromPathsToExsists.Count >= pathsList.Count)
	result.Add(flowsFromPathsToExsists);

CxList cydia = strings.FindByShortName("*cydia://*");
CxList canOpenURL = methods.FindByShortName("canOpenURL*");
result.Add(cydia.InfluencingOn(canOpenURL));


CxList privatePath = strings.FindByShortName("*/private/*");
CxList writeToFile = methods.FindByShortName("writeToFile*");
writeToFile.Add(methods.FindByShortName("writeToURL*"));
writeToFile.Add(methods.FindByShortName("write:*").FindByParameterName("toFile", 0));	
writeToFile.Add(methods.FindByShortName("write:*").FindByParameterName("to", 0));
writeToFile.Add(methods.FindByShortName("write:*").FindByParameterName("toUrl", 0));

result.Add(privatePath.InfluencingOn(writeToFile));