CxList methods = Find_Methods();
CxList sanitizers = 
	//DOM
	methods.FindByMemberAccess("DocumentBuilderFactory.setFeature", false) +
	//SAX
	methods.FindByMemberAccess("SAXParserFactory.setFeature", false) +
	// XMLReader 
	methods.FindByMemberAccess("XMLReader.setFeature", false) +
	// JDOM SAXBuilder
	methods.FindByMemberAccess("SAXBuilder.setFeature", false) +
	// DOM4J
	methods.FindByMemberAccess("SAXReader.setFeature", false) +
	// StAX
	methods.FindByMemberAccess("XMLInputFactory.setProperty", false);

result = sanitizers;