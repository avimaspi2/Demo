//Finds explicitly methods to sanitize XXE 
CxList methods = Find_Methods();
CxList sanitizers = All.NewCxList();

CxList sanitizing_properties =
	All.FindByMemberAccess("XmlReaderSettings.ProhibitDtd") +
	All.FindByMemberAccess("XmltextReader.ProhibitDtd");


foreach(CxList sanitizer in sanitizing_properties) {
	CxList assignment = sanitizer.GetAncOfType(typeof(AssignExpr));
	CxList assigned_value = All.GetByAncs(assignment).FindByShortName("true");
	if(assigned_value.Count > 0){
		sanitizers.Add(sanitizer);
	} 
}

sanitizing_properties = 
	All.FindByMemberAccess("XmlReaderSettings.DtdProcessing") +
	All.FindByMemberAccess("XmlTextReader.DtdProcessing");

foreach(CxList sanitizer in sanitizing_properties) {
	CxList assignment = sanitizer.GetAncOfType(typeof(AssignExpr));
	CxList assigned_value = 
		All.GetByAncs(assignment).FindByMemberAccess("DtdProcessing.Prohibit") +
		All.GetByAncs(assignment).FindByMemberAccess("DtdProcessing.Ignore");
	if(assigned_value.Count > 0){
		sanitizers.Add(sanitizer);
	} 
}


sanitizing_properties = 
	All.FindByMemberAccess("XmlReaderSettings.XmlResolver") +
	All.FindByMemberAccess("XmlTextReader.XmlResolver") +
	All.FindByMemberAccess("XmlDocument.XmlResolver");

foreach(CxList sanitizer in sanitizing_properties) {
	CxList assignment = sanitizer.GetAncOfType(typeof(AssignExpr));
	CxList assigned_value = 
		All.GetByAncs(assignment).FindByType(typeof(NullLiteral));
	if(assigned_value.Count > 0){
		sanitizers.Add(sanitizer);
	} 
}

CxList nameof = methods.FindByShortName("nameof");
sanitizers.Add(nameof);

result = sanitizers;