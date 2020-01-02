if (param.Length == 2)
{
	
	string packageName = param[0] as string;
	string versionName = param[1] as string;
	
	CxList gemlocks = Find_Gemlock_Versions();
	CxList binaryexps = Find_Gemlock_Versions_For_Package(gemlocks, packageName);
	result = Find_Gemlocks_Not_Satisfying_Version(binaryexps, versionName);

}