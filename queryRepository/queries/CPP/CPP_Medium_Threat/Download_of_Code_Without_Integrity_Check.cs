/// <summary>
/// This query checks for inputs (mainly .dll and .so libs) that are
///loaded into the code and are not checked for their integrity. 
/// </summary>

CxList inputs = Find_Inputs();
CxList loads = Find_LoadLibrary();

CxList sanitizers = Find_Checksums();
//Find the checksums that are actually applied on inputs
CxList check_inputs = sanitizers.InfluencedBy(inputs);

//Find if statements that are fathers of comparisons influenced by the checksums
CxList ifs_sanitizing = Find_BinaryExpr().InfluencedBy(check_inputs).GetAncOfType(typeof(IfStmt));

//Find the loads influenced by inputs
CxList loads_influenced = loads.InfluencedBy(inputs);

//exclude the loads that are not in the context of the ifs with sanitizers
result = loads_influenced - loads_influenced.GetByAncs(ifs_sanitizing);