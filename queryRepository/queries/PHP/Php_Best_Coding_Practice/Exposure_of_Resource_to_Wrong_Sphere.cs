CxList allFields = All.FindByType(typeof(FieldDecl));
CxList allPublicFields = allFields.FindByFieldAttributes(Modifiers.Public);
CxList allConstFields = allFields.FindByFieldAttributes(Modifiers.Readonly);

CxList suspiciousResults = allPublicFields - allConstFields;
CxList classDecl = All.FindByType(typeof(ClassDecl));
//Remove results that are on the same line of class declaretion 
//in order to remove implicit class variable declaration
List<KeyValuePair<string, int>> classList = new List<KeyValuePair<string, int>>();
foreach (CxList decl in classDecl){
	LinePragma lp = ((CSharpGraph) decl.data.GetByIndex(0)).LinePragma;
	classList.Add(new KeyValuePair<string, int>(lp.FileName,lp.Line));	
}
foreach (CxList res in suspiciousResults){
	LinePragma lp = ((CSharpGraph) res.data.GetByIndex(0)).LinePragma;
	if (!classList.Contains(new KeyValuePair<string, int>(lp.FileName,lp.Line)))
		result.Add(res);
}