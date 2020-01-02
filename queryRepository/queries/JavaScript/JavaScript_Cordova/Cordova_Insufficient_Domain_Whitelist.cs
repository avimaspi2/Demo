//This safeDomains collection serves to whitelist user given domains. Add your safe-to-whitelist URLS here. 
List<string> safeDomains = new List<string>();
if (param.Length >= 1 && param[0] is List<string>){
	safeDomains.AddRange(param[0] as List<string>);	
}

//Get all <allow-intent> tags
CxList whitelistedURLs = cxXPath.FindXmlNodesByLocalName("config.xml", 0, "allow-intent", true);
//Get all <access> tags
whitelistedURLs.Add(cxXPath.FindXmlNodesByLocalName("config.xml", 0, "access", true));
//Get all <allow-navigation> tags 
whitelistedURLs.Add(cxXPath.FindXmlNodesByLocalName("config.xml", 0, "allow-navigation", true));

CxList vulnerableWhitelistings = All.NewCxList();
Regex regEx = new Regex(@"^[\w-]+\:\*");
foreach(CxList element in whitelistedURLs)
{
	CxXmlNode xmlNode = element.TryGetCSharpGraph<CxXmlNode>();
	string hrefAttrName = xmlNode.GetAttributeValueByName("href");
	hrefAttrName = hrefAttrName.Equals(string.Empty) ? xmlNode.GetAttributeValueByName("origin") : hrefAttrName;
	if(
		!safeDomains.Exists(hrefAttrName.Contains) && 
		(hrefAttrName.StartsWith("*") || hrefAttrName.StartsWith("http://") || hrefAttrName.Contains("://*") || regEx.IsMatch(hrefAttrName))
		)
	{
		vulnerableWhitelistings.Add(element);
	}
}
result = vulnerableWhitelistings;