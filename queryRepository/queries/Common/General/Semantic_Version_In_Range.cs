/* 
Semantic_Version_In_Range(CxList nodes, String versionMin, String versionMax, String versionPrefix).

Filters `nodes` whose FullName match the pattern <versionPrefix><version> where:

	<versionPrefix> - the value of `versionPrefix`
	<version>       - a semantic versioning pattern (cf. semver.org) whose value «x» is such that 
                      versionMin <= x <= versionMax
*/
if (4 == param.Length)
{		
	Func<string, int, int[]> parseSemVer = (versionStr, d) => 
	{
		string[] mmp = versionStr.Split('.');
		int major = mmp.Length > 0 ? Int32.Parse(mmp[0]) : d;
		int minor = mmp.Length > 1 ? Int32.Parse(mmp[1]) : d;
		int patch = mmp.Length > 2 ? Int32.Parse(mmp[2]) : d;
		return new int[3] {major, minor, patch};	
	};
		
	Func<int[], int[], int> compareSemVer = (a, b) =>
	{
		int major = Math.Sign(a[0].CompareTo(b[0]));
		int minor = Math.Sign(a[1].CompareTo(b[1]));
		int patch = Math.Sign(a[2].CompareTo(b[2]));
		
		if (0 != major) return major;
		if (0 != minor) return minor;
		return patch;
	};
		
	CxList nodes = param[0] as CxList;
	int[] versionMin = parseSemVer(param[1] as string, Int32.MinValue);
	int[] versionMax = parseSemVer(param[2] as string, Int32.MinValue);
	string versionPrefix = param[3] as string;
	string regexStr = versionPrefix + @"(\d+\.\d+(\.\d+)?)";
	var regex = new System.Text.RegularExpressions.Regex(regexStr, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
	
	foreach (CxList node in nodes)
	{
		string nodeFullName = node.GetFirstGraph().FullName;
		var regexMatch = regex.Match(nodeFullName);
		if (regexMatch.Success)
		{
			string nodeVersionCapture = regexMatch.Groups[1].Value;
			int[] nodeVersion = parseSemVer(nodeVersionCapture, 0);
			int compareWithMin = compareSemVer(nodeVersion, versionMin);
			int compareWithMax = compareSemVer(nodeVersion, versionMax);
		
			if (0 <= compareWithMin && 0 >= compareWithMax)
			{
				result.Add(node);	
			}			
		}
	}
}