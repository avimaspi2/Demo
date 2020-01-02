CxList methods = Find_Methods();
CxList strings = Find_Strings();
List<string> pathsList = new List<string>() {
		"/system/app/Superuser.apk", 
		"/sbin/su", 
		"/system/bin/su", 
		"/system/xbin/su", 
		"/data/local/xbin/su", 
		"/data/local/bin/su", 
		"/system/sd/xbin/su",	
		"/system/bin/failsafe/su", 
		"/data/local/su", 
		"/su/bin/su"};
 
CxList paths = All.FindByShortNames(pathsList);

//https://www.npmjs.com/package/react-native-filesystem
CxList existsCalls = Find_Members("FileSystem.fileExists");
CxList flowsFromPathsToExsists = paths.InfluencingOn(existsCalls);

if (flowsFromPathsToExsists.Count >= pathsList.Count)
	result.Add(flowsFromPathsToExsists);


//https://www.npmjs.com/package/shelljs
CxList execCalls = methods.FindByShortName("exec");
CxList parameters = All.GetParameters(execCalls);
CxList su = strings.FindByShortName("su");
CxList which = strings.FindByShortName("/system/xbin/which");

CxList stringParametersPath = su.InfluencingOn(All.GetByAncs(parameters));

stringParametersPath = which.InfluencingOn(stringParametersPath.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));
result.Add(stringParametersPath);