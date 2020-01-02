CxList methods = Find_Methods();

CxList CodeProvider = All.FindByMemberAccess("CSharpCodeProvider.*", false);
CodeProvider.Add(All.FindByMemberAccess("VBCodeProvider.*", false));
CodeProvider.Add(All.FindByMemberAccess("MethodInfo.*", false));
CodeProvider.Add(All.FindByMemberAccess("JScriptCodeProvider.*", false));
CodeProvider.Add(All.FindByMemberAccess("CodeDomProvider.*", false));

//find only codeCompilers (clean anrelevant methods)
List < string > methCodeCompilers = new List<string> {"CompileAssemblyFrom*", "Parse"};
CxList codeCompilers = CodeProvider.FindByShortNames(methCodeCompilers, false);

CxList declarators = Find_Declarators();
CxList inputs = Find_DB_Out();
inputs.Add(Find_Read());
inputs.Add(All.FindAllReferences(inputs.GetAssignee().FindByType("*Reader", false)));
inputs.Add(declarators.FindDefinition(inputs.FindByType("DataSet", false)));

result = inputs.DataInfluencingOn(codeCompilers);