CxList methods = Find_Methods();

// 1 - Explicite DB function names
CxList directDbMethods =
	methods.FindByShortName("oci_execute") ;
	
result.Add(directDbMethods);

// Obsolete and alias names of DB functions
CxList directObsoleteAliasDbMethods =
	methods.FindByShortName("ociexecute") ;

result.Add(directObsoleteAliasDbMethods);