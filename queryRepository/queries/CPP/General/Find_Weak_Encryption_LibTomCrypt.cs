/**
 Find weak encryption from LibTomCrypt 1.18
 **/

CxList Methods = Find_Methods();
CxList Parameters = Find_Parameters();

// Vulnerable hashes
string[] VulnerableHashesPrefixes = new string[]{"sha1", "md5", "md2", "md4"};
foreach (string VulnHash in VulnerableHashesPrefixes)
{
	CxList VulnHash_Process = Find_Members_By_Include("tomcrypt.h", new string[]{VulnHash + "_process"});
	CxList VulnHash_Done = Find_Members_By_Include("tomcrypt.h", new string[]{VulnHash + "_done"});
	
	CxList VulnHash_Process_Ctx = All.GetParameters(VulnHash_Process, 0) - Parameters;
	CxList VulnHash_Done_InfluencedBy_Ctx = VulnHash_Done.DataInfluencedBy(VulnHash_Process_Ctx);
	
	foreach (CxList FromCtxToDone in VulnHash_Done_InfluencedBy_Ctx.GetCxListByPath())
	{
		CxList Ctx = FromCtxToDone.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
		CxList Process = VulnHash_Process.FindByParameters(Ctx);
		CxList Done = FromCtxToDone.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
		CxList Done_Output = All.GetParameters(Done, 1) - Parameters;
		
		CxList FromProcessToDone = Process.ConcatenateAllPaths(Done, false);
		CxList FromProcessToOutput = FromProcessToDone.ConcatenateAllPaths(Done_Output, false);
		
		result.Add(FromProcessToOutput);
	}
}