result = Find_Integers(); 
CxList methods = Find_Methods();
CxList unknownRefs = Find_UnknownReference();

result.Add(All.FindByMemberAccess("ResultSetMetaData.*")); 
result.Add(methods.FindByShortName("getClass*"));
result.Add(All.GetParameters(methods.FindByMemberAccess("Hashtable.get")));
result.Add(methods.FindByMemberAccess("URL.getProtocol"));
result.Add(methods.FindByMemberAccess("URL.getPort"));
result.Add(methods.FindByMemberAccess("FileInputStream.markSupported"));
result.Add(methods.FindByMemberAccess("*.setContentLength"));
result.Add(methods.FindByMemberAccess("Cookie.setMaxAge"));
result.Add(All.GetParameters(methods.FindByMemberAccess("EntityManager.find"), 1));

CxList HashSanitise = methods.FindByMemberAccess("MessageDigest.digest");
HashSanitise.Add(methods.FindByMemberAccess("MessageDigest.update"));
HashSanitise.Add(methods.FindByMemberAccess("MD5.getHash*"));
HashSanitise.Add(methods.FindByMemberAccess("MD5.update*"));
HashSanitise.Add(methods.FindByShortName("md5", false));
HashSanitise.Add(methods.FindByMemberAccess("Cipher.doFinal"));

CxList ESAPI = Find_ESAPI_Sanitizer();

// getAttribute
CxList getAttr = Get_Session_Attribute();
getAttr.Add(Get_Context_Attribute());

getAttr = All.GetParameters(getAttr);
CxList constants = Find_Constants();
constants.Add(getAttr.FindAllReferences(constants));
CxList strings = Find_Strings();

CxList allString = All.NewCxList();
allString.Add(strings);
allString.Add(strings.GetFathers());
allString.Add(constants);
allString.Add(constants.GetFathers());

getAttr -= allString;

result.Add(getAttr);

result.Add(HashSanitise);
result.Add(ESAPI);
result.Add(Find_Dead_Code_Contents());


//Methods that cut the flow of data
CxList getMethod = methods.FindByMemberAccess(".get");
CxList elementAtMethod = methods.FindByMemberAccess(".elementAt");
CxList removeMethod = methods.FindByMemberAccess(".remove");

CxList dataStractureGet = All.NewCxList();
	//.get
dataStractureGet.Add(getMethod.FindByMemberAccess("Attributes.get"));
dataStractureGet.Add(getMethod.FindByMemberAccess("Collection.get"));
dataStractureGet.Add(getMethod.FindByMemberAccess("List.get"));
dataStractureGet.Add(getMethod.FindByMemberAccess("Map.get"));
dataStractureGet.Add(getMethod.FindByMemberAccess("Table.get"));
dataStractureGet.Add(getMethod.FindByMemberAccess("Vector.get"));
	//.remove
dataStractureGet.Add(removeMethod.FindByMemberAccess("Attributes.remove"));
dataStractureGet.Add(removeMethod.FindByMemberAccess("Collection.remove"));
dataStractureGet.Add(removeMethod.FindByMemberAccess("List.remove"));
dataStractureGet.Add(removeMethod.FindByMemberAccess("Map.remove"));
dataStractureGet.Add(removeMethod.FindByMemberAccess("Table.remove"));
dataStractureGet.Add(removeMethod.FindByMemberAccess("Vector.remove"));
	//.elementAt
dataStractureGet.Add(elementAtMethod.FindByMemberAccess("Collection.elementAt"));
dataStractureGet.Add(elementAtMethod.FindByMemberAccess("Vector.elementAt"));
	
result.Add(All.GetParameters(dataStractureGet));

result.Add(Set_Context_Attribute());
result.Add(Set_Session_Attribute());
//result.Add(All.FindByMemberAccess("session.setAttribute")); // no direct flow from set to get attribute


result.Add(All.FindByMemberAccess("BASE64Encoder.encode"));
result.Add(All.FindByMemberAccess("Base64.encode"));
result.Add(All.FindByMemberAccess("HexBin.encode"));

//org.owasp.esapi.Encoder
List<string> owaspSanitizers = new List<string> {
		"forUriComponent",
		"encodeForBase64",
		"encodeForCSS",
		"encodeForHTML",
		"encodeForHTMLAttribute",
		"encodeForJavaScript",
		"encodeForURL",
		"encodeForXML",
		"encodeForXMLAttribute",
		"encodeForDN",
		"encodeForLDAP"};
CxList owaspEncoder = Get_ESAPI().FindByMemberAccess("Encoder.encode*").FindByShortNames(owaspSanitizers);
result.Add(owaspEncoder);

List<string> reformSanitizers = new List<string> {
		"HtmlAttributeEncode",
		"HtmlEncode",
		"JsString",
		"VbsString",
		"XmlAttributeEncode",
		"XmlEncode"};

CxList reformEncoder = All.FindByType("Reform").GetMembersOfTarget().FindByShortNames(reformSanitizers);
result.Add(reformEncoder);
CxList propertiesMethods = methods.FindByMemberAccess("Properties.getProperty");
CxList getParams = All.GetParameters(propertiesMethods, 0);
result.Add(methods.GetByAncs(getParams));