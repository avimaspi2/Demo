// Add input methods for mongodb DB

CxList unknownRef = Find_UnknownReference();
CxList requireOfDBMongo = Find_Require("mongodb");

List<String> allFilesWithDBMongo = new List<String>();
CxList allINSQLMongo = All.NewCxList();
foreach(CxList reqDB in requireOfDBMongo)
{
	try
	{
		CSharpGraph reqDBGR = reqDB.GetFirstGraph();
		String rDBName = reqDBGR.LinePragma.FileName;
		if (!allFilesWithDBMongo.Contains(rDBName))
		{
			allFilesWithDBMongo.Add(rDBName);
			allINSQLMongo.Add(All.FindByFileName(rDBName));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList methInvSQLMongo = allINSQLMongo.FindByType(typeof (MethodInvokeExpr));

// Only query() methods in files that include some DB
CxList sqlQueryMongo = methInvSQLMongo.GetMembersWithTargets().FindByShortNames(
	new List<string>{
		"drop",
		"findAndModify",
		"insert",
		"remove",
		"update"});

result.Add(sqlQueryMongo);

// Check if MongoDB is used in this project
if (requireOfDBMongo.Count > 0)
{
	// Find MongoDB by heuristics:
	CxList dbObjects = All.FindByShortName("db");
	
	CxList collections = dbObjects.GetMembersOfTarget().FindByShortNames(
		new List<string>{
			"collection",
			"allocations",
			"users",
			"contributions",
			"counters",
			"getCollection"});

	CxList assignRight = collections.FindByAssignmentSide(CxList.AssignmentSide.Right);
	CxList unknownAssign = assignRight.GetAssignee();
	collections.Add(unknownRef.FindAllReferences(unknownAssign));

	List<string> findMethodsNames = new List<string>{"findAndModify", "insert", "update", 
			"drop", "remove", "renameCollection", "save", "find", "findOne", 
			"findAndModify", "get*", "insertOne", "insertMany", "updateOne", 
			"updateMany", "deleteOne", "deleteMany", "findOneAndUpdate", 
			"findOneAndDelete", "findOneAndReplace", "bulkWrite", "replaceOne"};
	
	CxList findMethods = collections.GetMembersOfTarget().FindByShortNames(findMethodsNames);

	CxList irrelevant = Find_Param();
	irrelevant.Add(Find_LambdaExpr());
	result.Add(All.GetParameters(findMethods) - irrelevant); 
}