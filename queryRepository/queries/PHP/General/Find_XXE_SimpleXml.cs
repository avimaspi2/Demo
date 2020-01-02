CxList methods = Find_Methods();
CxList ObjConstructors = All.FindByType(typeof(ObjectCreateExpr));
CxList idents = All.FindByType(typeof(UnknownReference));

CxList Dtdload = idents.FindByShortName("LIBXML_DTDLOAD");

CxList xmlMethods = methods.FindByShortName("simplexml_load_file") + 
	methods.FindByShortName("simplexml_load_string");
CxList dangerousXmlMethods = xmlMethods * Dtdload.GetAncOfType(typeof(MethodInvokeExpr));

CxList xmlObjects = ObjConstructors.FindByShortName("SimpleXMLElement");
CxList dangerousInstances = xmlObjects * Dtdload.GetAncOfType(typeof(ObjectCreateExpr));

result = dangerousInstances + dangerousXmlMethods;