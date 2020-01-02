result = Find_Integers() + 
	All.FindByMemberAccess("ResultSetMetaData.*") +	  
	All.FindByShortName("getClass*") +
	All.FindByName("compareTo") + 
	All.FindByName("decode") + 
	All.GetParameters(All.FindByMemberAccess("Hashtable.get")) +
	All.FindByMemberAccess("URL.getProtocol") + 
	All.FindByMemberAccess("URL.getPort") +
	All.FindByMemberAccess("FileInputStream.markSupported") +
	All.FindByMemberAccess("*.setContentLength") + 
	All.FindByMemberAccess("Cookie.setMaxAge") +
	All.GetParameters(All.FindByMemberAccess("EntityManager.find"), 1)
	;

CxList HashSanitise = 
	All.FindByMemberAccess("MessageDigest.digest") + 
	All.FindByMemberAccess("MessageDigest.update") +
	All.FindByMemberAccess("MD5.getHash*") +
	All.FindByMemberAccess("MD5.update*") +
	Find_Methods().FindByShortName("md5", false) +
	All.FindByMemberAccess("Cipher.doFinal");

CxList ESAPI = Find_ESAPI_Sanitizer();

// getAttribute
CxList getAttr = Get_Session_Attribute() + Get_Context_Attribute();
getAttr = All.GetParameters(getAttr);
CxList constants = All.FindByType(typeof(ConstantDecl));
constants.Add(getAttr.FindAllReferences(constants));
CxList strings = Find_Strings();
getAttr -= strings + strings.GetFathers() + constants + constants.GetFathers();
result.Add(getAttr);

result.Add(HashSanitise + ESAPI);
result.Add(Find_Dead_Code_Contents());


//Methods that cut the flow of data
CxList getMethod = All.FindByMemberAccess(".get");
CxList elementAtMethod = All.FindByMemberAccess(".elementAt");
CxList removeMethod = All.FindByMemberAccess(".remove");

CxList dataStractureGet = 
	//.get
	getMethod.FindByMemberAccess("Attributes.get") + 
	getMethod.FindByMemberAccess("Collection.get") + 
	getMethod.FindByMemberAccess("List.get") + 
	getMethod.FindByMemberAccess("Map.get") + 
	getMethod.FindByMemberAccess("Table.get") +
	getMethod.FindByMemberAccess("Vector.get") + 
	//.remove
	removeMethod.FindByMemberAccess("Attributes.remove") + 
	removeMethod.FindByMemberAccess("Collection.remove") + 
	removeMethod.FindByMemberAccess("List.remove") + 
	removeMethod.FindByMemberAccess("Map.remove") + 
	removeMethod.FindByMemberAccess("Table.remove") +
	removeMethod.FindByMemberAccess("Vector.remove") + 
	//.elementAt
	elementAtMethod.FindByMemberAccess("Collection.elementAt") + 
	elementAtMethod.FindByMemberAccess("Vector.elementAt");
	

result.Add(All.GetParameters(dataStractureGet));

result.Add(Set_Context_Attribute());
result.Add(Set_Session_Attribute());
//result.Add(All.FindByMemberAccess("session.setAttribute")); // no direct flow from set to get attribute