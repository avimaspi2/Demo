List<string> badVersions = new List<string> { "0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17" };

CxList strings = Find_Strings();
CxList memberAccesses = Find_MemberAccesses();

CxList minSdkVer = memberAccesses.FindByShortName("ANDROID_MINSDKVERSION");
CxList versionLiterals = strings.DataInfluencingOn(minSdkVer);

result = versionLiterals.FindByShortNames(badVersions);