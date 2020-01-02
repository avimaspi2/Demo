CxList methods = Find_Methods();
CxList unknRefs = Find_UnknownReference();
result = methods.FindByMemberAccess("SmsMessage.get*");

// Add intents
result.Add(methods.FindByShortName("getIntent"));
result.Add(unknRefs.FindByShortName("intent"));

// Add fragment arguments
List<string> methodsNames = new List<string>(){"getArguments","arguments"};
CxList args = methods.FindByShortNames(methodsNames);
CxList frags = Find_Classes().FindByShortName("*Fragment*");
result.Add(args.GetByAncs(frags));

CxList editTextNodes = cxXPath.FindXmlNodesByLocalName("*.xml", 524288, "EditText", true);
editTextNodes.Add(cxXPath.FindXmlNodesByLocalName("*.xml", 524288, "AutoCompleteTextView", true));
editTextNodes.Add(cxXPath.FindXmlNodesByLocalName("*.xml", 524288, "MultiAutoCompleteTextView", true));

List<string> ids = new List<string>();
foreach(CxList editTextNode in editTextNodes) {
	CxXmlNode node = editTextNode.TryGetCSharpGraph<CxXmlNode>();
	string id = node.GetAttributeValueByName("id").Replace("@+id/", "");
	ids.Add(id);
}

// If we have ids defined in layout files, try to identify references for those layout elements
if (ids.Count > 0) {
	CxList editText = All.FindByTypes(new string []{
		"EditText",
		"AutoCompleteTextView",
		"MultiAutoCompleteTextView"
		});
	
	CxList editTextInputs = editText.GetMembersOfTarget().FindByShortNames(new List<string> {"text","getText"});
	
	CxList idsReferences = All.NewCxList();
	CxList texts = All.NewCxList();
	foreach(string id in ids) {
		idsReferences.Add(All.FindByName("R.id." + id));
		texts.Add(All.FindByMemberAccess(id + ".text"));
		texts.Add(All.FindByMemberAccess(id + ".getText"));
	}
	
	result.Add(editTextInputs);
	result.Add(texts);
	result.Add(editText * All.FindAllReferences(idsReferences.GetAncOfType(typeof(Declarator))).GetMembersOfTarget());
}