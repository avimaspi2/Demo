CxList methods = Find_Methods();

CxList sObjDB = methods.FindByShortName("insert") +
	methods.FindByShortName("update") +
	methods.FindByShortName("delete") +
	methods.FindByShortName("merge") +
	methods.FindByShortName("upsert") +
	methods.FindByShortName("undelete") +
	methods.FindByShortName("convertlead") + 
	methods.FindByShortName("select");

CxList encode = methods.FindByShortName("escapeSingleQuotes", false);

result = Find_Integers() + sObjDB + encode;

// replace 1st parameter
CxList repl = methods.FindByMemberAccess("string.replace") + 
	methods.FindByMemberAccess("string.replaceall") +
	methods.FindByMemberAccess("string.split");
result.Add(All.GetParameters(repl, 0));

// getters
CxList getters = (Find_VF_Pages() * Find_Methods()).FindByShortName("get*");
getters -= getters.FindByShortName("*__c");
getters -= getters.GetTargetOfMembers();

result.Add(getters 
	- getters.FindAllReferences(Find_Apex_Files().FindDefinition(getters))
	- (getters.GetTargetOfMembers() * Find_sObjects()).GetMembersOfTarget());