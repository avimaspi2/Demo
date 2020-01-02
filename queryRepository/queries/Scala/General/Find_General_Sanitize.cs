result = Find_Integers(); 
result.Add(All.FindByMemberAccess("ResultSetMetaData.*")); 
result.Add(All.FindByShortName("getClass*"));
result.Add(All.FindByName("compareTo"));
result.Add(All.FindByName("decode"));
result.Add(All.GetParameters(All.FindByMemberAccess("Hashtable.get")));
result.Add(All.FindByMemberAccess("URL.getProtocol"));
result.Add(All.FindByMemberAccess("URL.getPort"));
result.Add(All.FindByMemberAccess("FileInputStream.markSupported"));
result.Add(All.FindByMemberAccess("*.setContentLength"));
result.Add(All.FindByMemberAccess("Cookie.setMaxAge"));
result.Add(All.GetParameters(All.FindByMemberAccess("EntityManager.find"), 1));

CxList HashSanitise = All.FindByMemberAccess("MessageDigest.digest");
HashSanitise.Add(All.FindByMemberAccess("MessageDigest.update"));
HashSanitise.Add(All.FindByMemberAccess("MD5.getHash*"));
HashSanitise.Add(All.FindByMemberAccess("MD5.update*"));
HashSanitise.Add(Find_Methods().FindByShortName("md5", false));
HashSanitise.Add(All.FindByMemberAccess("Cipher.doFinal"));

CxList ESAPI = Find_ESAPI_Sanitizer();

result.Add(HashSanitise + ESAPI);

//Methods that cut the flow of data
CxList getMethod = All.FindByMemberAccess(".get");
CxList elementAtMethod = All.FindByMemberAccess(".elementAt");
CxList removeMethod = All.FindByMemberAccess(".remove");

CxList dataStructureGet = All.NewCxList();
	//.get
dataStructureGet.Add(getMethod.FindByMemberAccess("Attributes.get"));
dataStructureGet.Add(getMethod.FindByMemberAccess("Collection.get"));
dataStructureGet.Add(getMethod.FindByMemberAccess("List.get"));
dataStructureGet.Add(getMethod.FindByMemberAccess("Map.get"));
dataStructureGet.Add(Find_Map_Collection_Get() * getMethod);
dataStructureGet.Add(getMethod.FindByMemberAccess("Table.get"));
dataStructureGet.Add(getMethod.FindByMemberAccess("Vector.get"));
	//.remove
dataStructureGet.Add(removeMethod.FindByMemberAccess("Attributes.remove"));
dataStructureGet.Add(removeMethod.FindByMemberAccess("Collection.remove"));
dataStructureGet.Add(removeMethod.FindByMemberAccess("List.remove"));
dataStructureGet.Add(removeMethod.FindByMemberAccess("Map.remove"));
dataStructureGet.Add(removeMethod.FindByMemberAccess("Table.remove"));
dataStructureGet.Add(removeMethod.FindByMemberAccess("Vector.remove"));
	//.elementAt
dataStructureGet.Add(elementAtMethod.FindByMemberAccess("Collection.elementAt"));
dataStructureGet.Add(elementAtMethod.FindByMemberAccess("Vector.elementAt"));
	
result.Add(All.GetParameters(dataStructureGet));