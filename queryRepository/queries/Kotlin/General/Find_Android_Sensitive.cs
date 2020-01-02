// This query tries to find Android widgets with sensitive data (ex: EditText with passsword inputType),
// and find their relevant member accesses in the code;

CxList editTextNodes = cxXPath.FindXmlNodesByLocalName("*.xml", 524288, "EditText", true);
List<string> ids = new List<string>();
foreach(CxList editTextNode in editTextNodes) {
	CxXmlNode node = editTextNode.TryGetCSharpGraph<CxXmlNode>();
	string inputType = node.GetAttributeValueByName("inputType") + "";
	string id = node.GetAttributeValueByName("id").Replace("@+id/", "");
	if (inputType.ToLower().Contains("password")) {
		ids.Add(id);
	}
}

if (ids.Count > 0) {
	// Relevant calls
	CxList editText = All.FindByMemberAccess("EditText.getText");
	editText.Add(All.FindByMemberAccess("EditText.text"));
	
	CxList idsReferences = All.NewCxList();
	foreach(string id in ids) {
		idsReferences.Add(All.FindByName("R.id." + id));
	}
	CxList findByIdCalls = idsReferences.GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("findViewById");
	CxList fieldDefs = All.FindDefinition(All.DataInfluencedBy(findByIdCalls));
	result = editText * All.FindAllReferences(fieldDefs).GetMembersOfTarget();
}