CxList methods = Find_Methods();
CxList strings = Find_Strings();
List<string> pathsList = new List<string>() {"/system/app/Superuser.apk", 
		"/sbin/su", 
		"/system/bin/su", 
		"/system/xbin/su", 
		"/data/local/xbin/su", 
		"/data/local/bin/su", 
		"/system/sd/xbin/su",	
		"/system/bin/failsafe/su", 
		"/data/local/su", 
		"/su/bin/su"};
 
CxList paths = strings.FindByShortNames(pathsList);
CxList existsCalls = methods.FindByShortName("exists");
CxList flowsFromPathsToExists = paths.InfluencingOn(existsCalls);
if (flowsFromPathsToExists.Count >= pathsList.Count)
	result.Add(flowsFromPathsToExists);

CxList execCalls = methods.FindByShortName("exec");
CxList su = strings.FindByShortName("su");
CxList which = strings.FindByShortName("/system/xbin/which");

CxList stringParametersPath = su.InfluencingOn(execCalls);

stringParametersPath = which.InfluencingOn(stringParametersPath.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));
result.Add(stringParametersPath);

List<string> whichSuPaths= new List<string>() {
		"/system/xbin/which su", 
		"/system/bin/which su", 
		"which su"};
CxList whichSu = strings.FindByShortNames(whichSuPaths);
CxList flowsFromWhichToExec = whichSu.InfluencingOn(execCalls);
if(flowsFromWhichToExec.Count >= whichSuPaths.Count){
	result.Add(flowsFromWhichToExec);
}

CxList buildTags = All.FindByName("android.os.Build.TAGS");
CxList containsTestKeys = methods.FindByShortName("contains").FindByParameters(strings.FindByShortName("test-keys"));
result.Add( buildTags.InfluencingOn(containsTestKeys));
result -= Find_Dead_Code_AbsInt();