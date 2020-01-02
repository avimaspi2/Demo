CxList files = All.FindByFileName("*application.conf");
CxList members = files.GetMembersOfTarget();
CxList allowedOrigins = members.FindByShortName("allowedOrigins");
CxList corsEnabled = All.NewCxList();
if (allowedOrigins.Count > 0){
	CxList wildcards = files.FindByRegex("\"\\*\"");
	if (wildcards.Count > 0){
		foreach (CxList allowedOrigin in allowedOrigins){
			CSharpGraph originGraph = allowedOrigin.TryGetCSharpGraph<CSharpGraph>();
			corsEnabled.Add(wildcards.FindByPosition(originGraph.LinePragma.FileName, originGraph.LinePragma.Line));
			corsEnabled.Add(wildcards.FindByPosition(originGraph.LinePragma.FileName, originGraph.LinePragma.Line+1));
		}
	}
}
result = corsEnabled;