/* The presence of a webx.xml file indicates this is a Citrus project */
if(cxXPath.GetXmlFiles("webx.xml").Count() > 0)
{
	// We pass All to ensure the amount to results is greater than 0
	result = All;
}