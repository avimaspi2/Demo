// Finds the XQuery Injections that can be done on database
// This query may share some results with SQL_Injection
CxList inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();
CxList dbIns = Find_DB_In();
dbIns.Add(methods.FindByMemberAccess("XQPreparedExpression.execute*"));

CxList xmlQueryCandidates = Find_Strings().FindByShortNames(new List<string>{
		"*xmltable(*", "*xmltable (*", 
		"*select xmlquery(*", "*select xmlquery (*",
		"select xpath_exists(", "select xpath(", 
		"*.query(*", "*.query (*"
		}, false);

xmlQueryCandidates.Add(Find_Object_Create().FindByShortNames(new List<string>{
		"OXQDataSource", "OXQDDataSource", "DDXQDataSource" }));

CxList sanitize = Find_Sanitize();

CxList candidates = dbIns.DataInfluencedBy(xmlQueryCandidates);
// Jaxp
candidates.Add(methods.FindByMemberAccess("XQExpression.execute*"));
candidates.Add(methods.FindByMemberAccess("XQPreparedExpression.execute*"));
// Saxon
candidates.Add(methods.FindByMemberAccess("XQueryEvaluator.run*"));
candidates.Add(methods.FindByMemberAccess("XQueryExpression.evaluate*"));

result = candidates.InfluencedByAndNotSanitized(inputs, sanitize);