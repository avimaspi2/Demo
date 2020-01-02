// Work only if we get 2 parameters one for XML element namespace URI, and the other as the element name.
// Example 1: "sap.ui.ca" and "FileUpload"
// Example 2: "sap.m" and "UploadCollection"
// Example 3: "sap.ui.commons" and "FileUploader"

if(param.Length == 2)
{
	string namespaceURI = param[0] as string;	// xml element namespace URI
	string localName = param[1] as string;		// xml element name
	
	// generate xPath with element name and namespace
	string xPath = "//*[local-name()='" + localName + "' and namespace-uri() = '" + namespaceURI + "']";
	
	// search all XML files (*.xml)
	foreach (CxXmlDoc doc in cxXPath.GetXmlFiles("*xml")) 
	{ 
		try{
			// find all the node elements with the giving xPath (name and namespace)
			XPathNavigator navigator = doc.CreateNavigator();
			XPathNodeIterator nodeIterator = navigator.Select(xPath);
			while (nodeIterator.MoveNext())
			{
				// add the select node to result
				XPathNavigator currentNodeNavigator = nodeIterator.Current;
				result.Add(cxXPath.CreateXmlNode(currentNodeNavigator, doc, 2, false));
			}
		} 
		catch(Exception){}
	}
}