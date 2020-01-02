//indexed db
CxList objectStore = All.FindByShortName("*ObjectStore", false);
CxList invokes = Find_Methods();
CxList dbIn = invokes.FindByShortNames(new List<string>{"add", "put"});
CxList allDbIns = All.NewCxList();
allDbIns.Add(dbIn * dbIn.DataInfluencedBy(objectStore));

// deprecated web sql database
allDbIns.Add(invokes.FindByShortName("executeSql"));

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
//Collection.update(query, update, options, callback);
//Collection.updateById(id, update, options, callback);
//Collection.insert(data, index, callback);
List<string> inputNames = new List<string> {"update", "updateById", "setData", "insert", "find", "findOne", "findById"};
CxList collectionMembers = runnerCollection.GetMembersOfTarget();
CxList collectionFind = collectionMembers.FindByShortNames(inputNames);
allDbIns.Add(All.GetParameters(collectionFind, 0));
allDbIns.Add(All.GetParameters(collectionFind, 1));
//collection.findSub(match, path, subDocQuery, subDocOptions);
//collection.findSubOne(match, path, subDocQuery, subDocOptions);
List<string> FindSubNames = new List<string> {"findSub", "findSubOne"};
CxList collectionFindSub = collectionMembers.FindByShortNames(FindSubNames);
allDbIns.Add(All.GetParameters(collectionFindSub, 0));
allDbIns.Add(All.GetParameters(collectionFindSub, 1));
allDbIns.Add(All.GetParameters(collectionFindSub, 2));

//typeORM
CxList repositories = invokes.FindByShortName("getRepository");
allDbIns.Add(repositories.GetMembersOfTarget().FindByShortName("createQueryBuilder"));

allDbIns -= Find_Param();

allDbIns.Add(Backbone_Find_DB_In());
	
result = allDbIns;

result.Add(NodeJS_Find_DB_IN());