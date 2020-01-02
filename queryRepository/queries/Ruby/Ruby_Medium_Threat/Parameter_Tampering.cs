CxList input = Find_Interactive_Inputs();
CxList db = Find_DB();
CxList strings = Find_Strings();

CxList UnknownReferences = All.FindByType(typeof(UnknownReference));

// Operations on SQL databases
CxList Select = strings.FindByName("*select*", false);
CxList Where = strings.FindByName("*where*", false);
CxList And = strings.FindByName("*And *", false) + 
	strings.FindByName("* And*", false);

// Operations on ActiveRecord interface
CxList find_by = 
	db.FindByShortName("find_by*") +
	db.FindByShortName("scoped_by*") +
	db.FindByShortName("find_or_initialize_by*") +
	db.FindByShortName("find_or_create_by*") +
	db.FindByShortName("find_all_by*") +
	db.FindByShortName("find_last_by*");
find_by -= find_by.FindByShortName("find_by_sql");
find_by -= find_by.FindByShortName("*_and_*");

// Operations on MongoDB
CxList MongoDb = Find_DB_Out_Mongo();

db = db.DataInfluencedBy(Select).DataInfluencedBy(Where);
db -= db.DataInfluencedBy(And);
db.Add(find_by);
db.Add(MongoDb);

// Find potential vulnerabilities as flows
CxList potentialVulnerabilities = input.DataInfluencingOn(db);

// Now, we want to filter out flows for which there is a check

// Find everything in IfStmt conditions
CxList IfStatements = All.FindByType(typeof(IfStmt));
CxList conditions = All.FindByFathers(IfStatements);
conditions -= conditions.FindByType(typeof(StatementCollection));
conditions = All.GetByAncs(conditions);
// null-check is not considered as a good-enough check
CxList nil = conditions.FindByShortName("nil?");
conditions -= nil;
conditions -= nil.GetTargetOfMembers();

// Find flows from inputs to check locations
CxList inputChecks = input.DataInfluencingOn(conditions);

// Iterate on all flow in potentialVulnerabilities
foreach (CxList vul in potentialVulnerabilities.GetCxListByPath())
{
	CxList sink = vul.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
	bool foundCheck = false;
	// If the input is checked in an enclosing IfStmt, consider there is no vulnerability
	foreach (CxList inputCheck in inputChecks.GetCxListByPath())
	{
		CxList checkSource = inputCheck.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
		CxList checkSink = inputCheck.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
		
		// Cope with indexers (for instance x[:y])
		CxList vulSource = vul + All.FindByFathers(vul.GetFathers().FindByType(typeof(IndexerRef)));
		if(vulSource.Count > 1){
			checkSource = checkSource + UnknownReferences.FindByFathers(checkSource.GetFathers().FindByType(typeof(IndexerRef)));
		}
		
		// Consider only check of current input
		if((vulSource.FindAllReferences(checkSource)).Count != vulSource.Count){
			continue;
		}
		CxList checkingIfStmt = checkSink.GetAncOfType(typeof(IfStmt));
		// If the sink is control-influenced by the checking condition
		if(sink.GetByAncs(checkingIfStmt).Count > 0){
			foundCheck = true;
			break;
		}
	}
	// If there is no check ... consider as a vulnerability
	if(!foundCheck){
		result.Add(vul);
	}
}