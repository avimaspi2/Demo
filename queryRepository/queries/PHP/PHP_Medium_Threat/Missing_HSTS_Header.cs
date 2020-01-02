bool show_only_one_HSTS_result = true;
CxList stringLiterals = Find_Strings();
CxList globalConfig = Find_HSTS_Configuration_In_Code();
if(globalConfig.Count>0){
	result = Check_HSTS_Configuration();
}
else{
	CxList headerChanges = Find_Methods().FindByShortNames(new List<string>(){"header","setHeader"});
	
	CxList paramsOfHeader = stringLiterals.GetParameters(headerChanges);
	CxList hstsHeaders = paramsOfHeader.FindByShortName("*Strict-Transport-Security*", false);
	if(hstsHeaders.Count == 0){
		CSharpGraph res = All.GetFirstGraph();
		if(res != null){result.Add(res.NodeId, res);}	
		}
	else{
		CxList badHSTSHeaders = All.NewCxList();
		foreach(CxList header in hstsHeaders){
			if(Validate_HSTS_Header(header).Count > 0){
				result.Add(headerChanges.InfluencedBy(header));
				if(show_only_one_HSTS_result){
					break;
				}
			}
		}
	}
}