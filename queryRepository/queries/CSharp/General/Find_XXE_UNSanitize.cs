//Finds methods that explicitly unsanitize XXE 
CxList UNsanitizers = All.NewCxList();

CxList UNsanitizing_properties =
	All.FindByMemberAccess("XmlReaderSettings.ProhibitDtd") +
	All.FindByMemberAccess("XmltextReader.ProhibitDtd");


foreach(CxList UNsanitizer in UNsanitizing_properties) {
	CxList assignment = UNsanitizer.GetAncOfType(typeof(AssignExpr));
	CxList assigned_value = All.GetByAncs(assignment).FindByShortName("false");
	if(assigned_value.Count > 0){
		UNsanitizers.Add(UNsanitizer);
	} 
}


UNsanitizing_properties = 
	All.FindByMemberAccess("XmlReaderSettings.DtdProcessing") +
	All.FindByMemberAccess("XmlTextReader.DtdProcessing");

foreach(CxList UNsanitizer in UNsanitizing_properties) {
	CxList assignment = UNsanitizer.GetAncOfType(typeof(AssignExpr));
	CxList assigned_value = 
		All.GetByAncs(assignment).FindByMemberAccess("DtdProcessing.Parse");
	if(assigned_value.Count > 0){
		UNsanitizers.Add(UNsanitizer);
	} 
}

UNsanitizing_properties = 
	All.FindByMemberAccess("XmlReaderSettings.XmlResolver") +
	All.FindByMemberAccess("XmlTextReader.XmlResolver") +
	All.FindByMemberAccess("XmlDocument.XmlResolver");

foreach(CxList UNsanitizer in UNsanitizing_properties) {
	CxList assignment = UNsanitizer.GetAncOfType(typeof(AssignExpr));
	CxList assigned_value = 
		All.GetByAncs(assignment).FindByType(typeof(NullLiteral));
	if(assigned_value.Count == 0){
		UNsanitizers.Add(UNsanitizer);
	} 
}


result = UNsanitizers;