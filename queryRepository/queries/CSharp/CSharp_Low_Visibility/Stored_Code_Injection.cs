//find all code provider
CxList CodeProvider = All.FindByMemberAccess("CSharpCodeProvider.*");
CodeProvider.Add(All.FindByMemberAccess("VBCodeProvider.*"));
CodeProvider.Add(All.FindByMemberAccess("MethodInfo.*"));
CodeProvider.Add(All.FindByMemberAccess("JScriptCodeProvider.*"));
CodeProvider.Add(All.FindByMemberAccess("CodeDomProvider.*"));

//find only codeCompilers (clean irrelevant methods)
List < string > methCodeCompilers = new List<string> {"CompileAssemblyFrom*", "Parse", "Invoke"};
CxList codeCompilers = CodeProvider.FindByShortNames(methCodeCompilers, false);

CxList inputs = Find_Read() + Find_DB_Out();
CxList sanitize = Find_Sanitize();
result = inputs.InfluencingOnAndNotSanitized(codeCompilers, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);