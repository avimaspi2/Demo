CxList strings = Find_Strings();
List<string> pathsList = new List<string>() {
		"*/Applications/Cydia.app*",
		"*/Library/MobileSubstrate/MobileSubstrate.dylib*",
		"*/bin/bash*",
		"*/usr/sbin/sshd*",
		"*/etc/apt*",
		"*/private/var/lib/apt/*"};
 
CxList paths = All.FindByShortNames(pathsList);

//https://www.npmjs.com/package/react-native-filesystem
CxList existsCalls = Find_Members("FileSystem.fileExists");
CxList flowsFromPathsToExsists = paths.InfluencingOn(existsCalls);

if (flowsFromPathsToExsists.Count >= pathsList.Count)
	result.Add(flowsFromPathsToExsists);

CxList cydia = strings.FindByShortName("*cydia://*");
CxList canOpenURL = Find_Members("Linking.canOpenURL");
result.Add(cydia.InfluencingOn(canOpenURL));


CxList privatePath = strings.FindByShortName("*/private/*");
CxList writeToFile = Find_Members("FileSystem.writeToFile");

result.Add(privatePath.InfluencingOn(writeToFile));