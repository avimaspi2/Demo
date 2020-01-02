CxList inputs = Find_Android_Interactive_Inputs();
CxList methods = Find_Methods();
CxList parameters = Find_Param();

//code sanitized
CxList parametersFalse = parameters.FindByShortName("false");

List<string> methodsName = new List<string>{"setLongClickable","setTextIsSelectable"};

CxList methodInvoke = methods.FindByShortNames(methodsName).FindByParameters(parametersFalse);
methodInvoke.Add(methods.FindByShortName("setCustomSelectionActionModeCallback"));

CxList inputSanitizedByCode = methodInvoke.GetTargetOfMembers();

CxList declaratorsSanitized =  Find_Declarators().FindDefinition(inputSanitizedByCode);
CxList elementSanitized = declaratorsSanitized.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly).GetTargetOfMembers();

//build a list with elements
List<string> listEditTextSanitized = new List<string>();
foreach(CxList elem in elementSanitized) {
	CSharpGraph edtiTextElement = elem.TryGetCSharpGraph<CSharpGraph>();
	listEditTextSanitized.Add(edtiTextElement.ResolvedShortName);	
}

//xml sanitized
CxList editTextNodes = cxXPath.FindXmlNodesByLocalName("*.xml", 524288, "EditText", true);
CxList sanitized = All.NewCxList();
foreach(CxList editTextNode in editTextNodes) {
	CxXmlNode node = editTextNode.TryGetCSharpGraph<CxXmlNode>();
	string id = node.GetAttributeValueByName("id").Replace("@+id/", "");
	if( node.GetAttributeValueByName("inputType") == "textPassword" || node.GetAttributeValueByName("longClickable") == "false" || node.GetAttributeValueByName("textIsSelectable") == "false" || listEditTextSanitized.Contains(id)){
		sanitized.Add(editTextNode);
	}
}
result = editTextNodes - sanitized;