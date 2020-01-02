/* Citrus Webx: Look for productionMode flag set to false*/

foreach (CxXmlDoc doc in cxXPath.GetXmlFiles("webx.xml")) 
{
	XPathNavigator navigator = doc.CreateNavigator();
	XPathNodeIterator nodeIterator = navigator.Select("//productionMode");
	while (nodeIterator.MoveNext())
	{
		XPathNavigator currentNodeNavigator = nodeIterator.Current;
		if (currentNodeNavigator.Value.Contains("false"))
		{
			result.Add(cxXPath.CreateXmlNode(currentNodeNavigator, doc, 2, false));
		}
	}
}