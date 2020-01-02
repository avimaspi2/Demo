CxList invokes = Find_Methods();

//indexed db
CxList dbResult = All.FindByShortName("result");
CxList dbResultMember = dbResult.GetTargetOfMembers();
CxList allDbOuts = All.NewCxList();

CxList transaction = invokes.FindByShortName("get", false);
transaction.Add(invokes.FindByShortName("getAll"));

CxList allRefs = All.FindByShortName(dbResultMember).FindAllReferences(dbResultMember);
dbResultMember = dbResultMember.FindAllReferences(allRefs.DataInfluencedBy(transaction));
allDbOuts.Add(dbResultMember.GetMembersOfTarget());

//event driven response

CxList allEvents = dbResult.GetTargetOfMembers().GetTargetOfMembers();
foreach(CxList events in allEvents)
{
	CxList methodsWithEventsAsArguments = All.GetMethod(events);
	methodsWithEventsAsArguments.Add(events.GetAncOfType(typeof(LambdaExpr)));
	CxList parameter = All.FindByShortName(events).FindAllReferences(events).GetParameters(methodsWithEventsAsArguments);
	
	CxList methodsOfParameter = All.GetMethod(parameter);
	methodsOfParameter.Add(parameter.GetAncOfType(typeof(LambdaExpr)));
	CxList invokesAndLambdas = All.NewCxList();
	invokesAndLambdas.Add(invokes);
	invokesAndLambdas.Add(Find_LambdaExpr());
	CxList methodInvoke = invokesAndLambdas.FindAllReferences(methodsOfParameter).FindByType(typeof(LambdaExpr));
	
	CxList transactionMultiInvoke = invokes.FindByShortName("get").GetMembersOfTarget().GetAssigner(methodInvoke);
	if(transactionMultiInvoke.Count > 0)
	{
		allDbOuts.Add(events.GetMembersOfTarget().GetMembersOfTarget());
	}
}
//now we need to handle cursor.
CxList val = Find_Members("*.value");
dbResult = dbResult.GetTargetOfMembers();

CxList dbResultTargets = dbResult.FindByShortName("*request", false);
dbResultTargets.Add(dbResult.FindByShortName("target"));

CxList dbResultMembers = dbResultTargets.GetMembersOfTarget();

CxList cursor = val.GetTargetOfMembers();

CxList allCursors = All.FindByShortName(cursor).FindAllReferences(cursor);

CxList temp = (allCursors.DataInfluencedBy(dbResultMembers));
CxList hasMember = cursor.FindAllReferences(temp).GetMembersOfTarget();
CxList targetsOfHasMember = hasMember.GetMembersOfTarget();
CxList noMember = hasMember - targetsOfHasMember.GetTargetOfMembers();
allDbOuts.Add(targetsOfHasMember);
allDbOuts.Add(noMember);

//deprecated web-SQL database

CxList executeSQL = invokes.FindByShortName("executeSql");
CxList functionPointer = All.GetParameters(executeSQL, 2);

//this is in case the function itself is passed as a parameter
CxList invokeOfSQLFunc = All.FindDefinition(functionPointer);
CxList target = All.FindAllReferences(All.GetParameters(invokeOfSQLFunc, 1));

CxList tar = target.GetMembersOfTarget().GetMembersOfTarget().GetMembersOfTarget();

allDbOuts.Add(tar);
allDbOuts.Add(target);
allDbOuts.Add(executeSQL);

// forerunnerDB
/*
var fdb = new ForerunnerDB(),
	db = fdb.db('test'),
	items = db.collection('items');
*/
// eliminate files of server-side code
CxList unknownReferences = Find_UnknownReference();
CxList require = Find_Import();
require -= require.GetMembersWithTargets();
CxList forerunnerDB = Find_ObjectCreations().FindByShortName("ForerunnerDB");
forerunnerDB -= forerunnerDB.FindByFiles(require);
forerunnerDB.Add(unknownReferences.FindAllReferences(forerunnerDB.GetAssignee()));
CxList runnerDB = forerunnerDB.GetMembersOfTarget().FindByShortName("db");
runnerDB.Add(unknownReferences.FindAllReferences(runnerDB.GetAssignee()));
CxList runnerCollection = runnerDB.GetMembersOfTarget().FindByShortName("collection");
runnerCollection.Add(unknownReferences.FindAllReferences(runnerCollection.GetAssignee()));
//collection.findById(id, options);
//collection.find(query, options, callback);
//collection.findOne();
//collection.findSub(match, path, subDocQuery, subDocOptions);
//collection.findSubOne(match, path, subDocQuery, subDocOptions);
List<string> FindNames = new List<string> {"find", "findOne", "findById", "findSub", "findSubOne"};
CxList collectionMembers = runnerCollection.GetMembersOfTarget();
allDbOuts.Add(collectionMembers.FindByShortNames(FindNames));

//typeORM
CxList repositories = invokes.FindByShortName("getRepository");
allDbOuts.Add(repositories.GetMembersOfTarget().FindByShortName("createQueryBuilder"));

allDbOuts.Add(Backbone_Find_DB_Out()); 
allDbOuts.Add(NodeJS_Find_Sequelize_DB_Out());
result = allDbOuts;