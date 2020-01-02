/// <summary>
/// This query checks if the flag max-age 
/// is present in configuration file 
/// </summary>

CxList appConf = Find_Hocon_Application_Conf().FindByType(typeof(MemberAccess));
CxList sessions = appConf.ReduceFlowByPragma().FindByName("*session*");

List<String> knownFiles = new List<string>();
CxList integers = All.FindByType(typeof(IntegerLiteral));

foreach(CxList session in sessions){
	try{
		CSharpGraph g = session.TryGetCSharpGraph<CSharpGraph>();
		String currentFile = g.LinePragma.FileName;
		if(!knownFiles.Contains(currentFile)){
			knownFiles.Add(currentFile);
			CxList maxages = appConf.FindByFileName(currentFile).FindByName("*session.max-age");
			if(maxages.Count == 0){
				result.Add(session);
			}else{
				CxList maxages_value = maxages.GetAssigner(integers);
				foreach(CxList integer in maxages_value ){
					IntegerLiteral x = integer.TryGetCSharpGraph<CSharpGraph>() as IntegerLiteral;
					if (x.Value != 1){
						result.Add(session);
					}
				}
			}
		}
	}catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}	
}