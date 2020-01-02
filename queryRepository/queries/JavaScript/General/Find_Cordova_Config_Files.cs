/**
*return all the config file relative a cordova project
*<widget id="com.example.hello" version="1.0.0" xmlns="http://www.w3.org/ns/widgets" xmlns:cdv="http://cordova.apache.org/ns/1.0">
**/
try{
	CxList configXml = cxXPath.FindXmlNodesByLocalName("config.xml", 0, "widget", true);
	string cdvAttr = null;
	foreach(CxList myFile in configXml){
	
		CxXmlNode xmlNode = myFile.TryGetCSharpGraph<CxXmlNode>();
		cdvAttr = xmlNode.GetAttributeValueByName("cdv");
	
		if( null != cdvAttr && cdvAttr.Contains("http://cordova.apache.org")){
			result.Add(myFile);
		}
	}
}catch(Exception e){}