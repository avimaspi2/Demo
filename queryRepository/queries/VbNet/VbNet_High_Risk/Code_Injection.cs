CxList CodeProvider = All.FindByMemberAccess("CSharpCodeProvider.*", false);
CodeProvider.Add(All.FindByMemberAccess("VBCodeProvider.*", false));
CodeProvider.Add(All.FindByMemberAccess("JScriptCodeProvider.*", false));
CodeProvider.Add(All.FindByMemberAccess("CodeDomProvider.*", false));
//find only codeCompilers (clean anrelevant methods)
List < string > methCodeCompilers = new List<string> {"CompileAssemblyFrom*", "Parse"};
CxList codeCompilers = CodeProvider.FindByShortNames(methCodeCompilers, false);

result = Find_Interactive_Inputs().DataInfluencingOn(codeCompilers);