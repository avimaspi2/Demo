// Finds the XQuery Injections that can be done on database
// This query may share some results with SQL_Injection
CxList inputs = Find_Interactive_Inputs();
CxList dbIns = Find_DB_In();

dbIns.Add(Find_XQuery_Injection_Additional_DBIn());

CxList xmlQueryCandidates = Find_XQuery_Injection_Xml_Query_Candidates();

CxList sanitize = Find_Sanitize();

CxList candidates = dbIns.DataInfluencedBy(xmlQueryCandidates);
// Jaxp and Saxon
candidates.Add(Find_XQuery_Injection_Candidates());

result = candidates.InfluencedByAndNotSanitized(inputs, sanitize);