CxList inputs = Find_Inputs();
CxList methods = Find_Methods();
CxList sanitizers = Find_Sanitize();
inputs.Add(Find_Read());

CxList insecureMethods = All.NewCxList();

List<string> pickleMethodNames = new List<string>{
		"load", "loads", "noload", "Unpickler"};
CxList pickleMemberAccess = methods.FindByMemberAccess("pickle.*");
insecureMethods.Add(pickleMemberAccess.FindByShortNames(pickleMethodNames));
insecureMethods.Add(methods.FindByMemberAccess("shelve.open"));

List<string> hashMethodNames = new List<string>{
		"md5", "sha256"};
CxList hashMemberAccess = methods.FindByMemberAccess("hashlib.*");
CxList hash = hashMemberAccess.FindByShortNames(hashMethodNames);

CxList IfsWithInsecureMethods = insecureMethods.GetAncOfType(typeof(IfStmt));
CxList conditions = All.FindByFathers(IfsWithInsecureMethods).FindByType(typeof(Expression));
CxList safeConditions = conditions.DataInfluencedBy(hash).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList safe = insecureMethods.GetByAncs(safeConditions.GetAncOfType(typeof(IfStmt)));

insecureMethods -= safe;

inputs -= insecureMethods; 

result = insecureMethods.InfluencedByAndNotSanitized(inputs, sanitizers);