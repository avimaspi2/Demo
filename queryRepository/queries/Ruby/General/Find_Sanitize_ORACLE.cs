CxList commands = Find_DB_In_ORACLE();

CxList commands0 = 
	commands.FindByShortName("all") +
	commands.FindByShortName("delete") +
	//commands.FindByShortName("exec") +
	commands.FindByShortName("execute") +
	commands.FindByShortName("first") +
	commands.FindByShortName("one") +
	commands.FindByShortName("parse") +
	commands.FindByShortName("select_all") +
	commands.FindByShortName("select_first") +
	commands.FindByShortName("select_hash_all") +
	commands.FindByShortName("select_hash_first");

CxList commands1 = 
	commands.FindByShortName("select");

CxList commandsParams = All.GetByAncs(All.GetParameters(commands));
CxList nonSanitizers = 
	commandsParams.GetByAncs(All.GetParameters(commands0, 0)) +
	commandsParams.GetByAncs(All.GetParameters(commands1, 1));

result = commandsParams - nonSanitizers;


CxList methods = Find_Methods();

CxList relevantSanitize = 
	methods.FindByShortName("bind_param") +
	All.FindByShortName("OCI8").FindByType(typeof(ObjectCreateExpr));
	
result.Add(All.GetByAncs(relevantSanitize));