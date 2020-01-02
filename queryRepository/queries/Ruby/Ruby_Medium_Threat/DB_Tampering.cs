CxList commands = Find_MongoDB_Def().GetMembersOfTarget();

// 1. drop database
CxList dangerousCommands = commands.FindByShortName("drop_database");
CxList commandParams = All.GetByAncs(All.GetParameters(dangerousCommands, 0));

CxList inputs = Find_Interactive_Inputs() + Find_Strings();
CxList sanitize = Find_Integers();

// 2. tamper with administrative rights
CxList admin = commands.FindByShortName("admin");
CxList adminRef = All.GetByAncs(admin.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
adminRef = All.FindAllReferences(adminRef) - admin;
adminRef.Add(adminRef.GetMembersOfTarget());
adminRef.Add(adminRef.GetMembersOfTarget());

commandParams.Add(adminRef);
sanitize.Add(admin);

// 3. createCollection
CxList createCollection = commands.FindByShortName("createCollection");
commandParams.Add(All.GetParameters(createCollection));

result = 
	inputs * commandParams +
	commandParams.InfluencedByAndNotSanitized(inputs, sanitize);