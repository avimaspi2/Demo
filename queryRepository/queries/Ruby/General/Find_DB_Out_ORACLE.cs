CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods = 
	methods.FindByShortName("oci_fetch_array") + 
	methods.FindByShortName("oci_fetch_assoc") + 
	methods.FindByShortName("oci_fetch_object") +
	methods.FindByShortName("oci_fetch_row") +
	methods.FindByShortName("oci_result");
result.Add(directDbMethods);

// Explicit DB obsolete or alias function names
CxList directObsoleteAliasDbMethods = 
	methods.FindByShortName("ocicollassignelem") + 
	methods.FindByShortName("ocifetch") + 
	methods.FindByShortName("ociloadlob") +
	methods.FindByShortName("ociresult");
result.Add(directObsoleteAliasDbMethods);

// 2 - Find query\execute function calling on object initialize with DB.
CxList pCollCreation = methods.FindByShortName("oci_new_collection", false);
CxList getElemCalls = methods.FindByShortName("getElem", false);
CxList getCollElemGet = getElemCalls.InfluencedBy(pCollCreation);

result = getElemCalls;