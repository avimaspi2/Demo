CxList imports = Find_Imports();

//libxml2
String[] libXml2Methods = new string[]{"*xpathEval"};
CxList libXml2 = Find_Methods_By_Import("libxml2", libXml2Methods, imports); 

//lxml
String[] lxmlMethods = new string[]{"*xpath","*xpathEvaluator"};
CxList lxml = Find_Methods_By_Import("lxml*", lxmlMethods, imports);

//ElementTree
String[] elemTree = new string[]{"*find","*findall","*findtext", "*iterfind"};
CxList elementTree = Find_Methods_By_Import("xml.etree.ElementTree", elemTree, imports); 

//py-dom-xpath
CxList pyDomXpath = Find_PY_DOM_XPath();

//py-xml
String[] pyxml = new string[]{"*Evaluate"};
CxList pyXml = Find_Methods_By_Import("xml", pyxml, imports); 


CxList xPath = All.NewCxList();
xPath.Add(libXml2);
xPath.Add(lxml);
xPath.Add(pyDomXpath);
xPath.Add(pyXml);
xPath.Add(elementTree);

CxList inputs = Find_Interactive_Inputs();

CxList sanitized = Find_Sanitize();
sanitized.Add(Find_PY_DOM_XPath_Sanitizers(pyDomXpath));

result = xPath.InfluencedByAndNotSanitized(inputs, sanitized);