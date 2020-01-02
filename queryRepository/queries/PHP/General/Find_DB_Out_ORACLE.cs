CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = methods.FindByShortNames(new List<string> {"oci_fetch_array", "oci_fetch_assoc", "oci_fetch_object", 
		"oci_fetch_row", "oci_result"});

result.Add(directDbMethods);

// Explicit DB obsolete or alias function names
CxList directObsoleteAliasDbMethods = methods.FindByShortNames(new List<string> {"ocifetch", "ociloadlob", "ociresult"});

result.Add(directObsoleteAliasDbMethods);

// 2 - Find query\execute function calling on object initialize with DB.
CxList pCollCreation = methods.FindByShortName("oci_new_collection", false);
CxList getElemCalls = methods.FindByShortName("getElem", false);
CxList getCollElemGet = getElemCalls.InfluencedBy(pCollCreation);

result.Add(getCollElemGet);


//find "oci_fetch_all" methods and add the second output parameter
CxList fetchAllMethods = methods.FindByShortName("oci_fetch_all");
CxList fetchAllParams = All.GetParameters(fetchAllMethods, 1);
result.Add(fetchAllParams);