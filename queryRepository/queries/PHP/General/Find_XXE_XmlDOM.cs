CxList methods = Find_Methods();

CxList loadDOM = methods.FindByMemberAccess("DOMDocument.loadXML") + 
	methods.FindByMemberAccess("DOMDocument.load");

result = loadDOM;