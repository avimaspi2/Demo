result = Find_Plain_DB_Out();

//Find_DB_Out for MongoDB
CxList dbDef = All.FindByName("*mongodb.Mongo");
dbDef.Add(All.FindByName("*mongodb.MongoURI"));
dbDef.Add(All.FindByName("*mongodb.casbah"));

dbDef = dbDef.FindByType(typeof(ObjectCreateExpr)) + dbDef.FindByType(typeof(MemberAccess));

CxList dbAll = All.FindAllReferences(dbDef);
dbAll.Add(All.GetByAncs(dbAll.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left));
dbAll.Add(All.FindAllReferences(dbAll));
dbAll.Add(All.FindByType(typeof(UnknownReference)).InfluencedByAndNotSanitized(dbAll, All.FindByType(typeof(AssignExpr)) + All.FindByType(typeof(BinaryExpr))));

CxList dbBasicCommands = dbAll.GetMembersOfTarget();
CxList dbCollections = dbBasicCommands.FindByShortName("collection"); 
dbCollections.Add(dbAll.FindByType(typeof(IndexerRef)));

dbCollections.Add(All.FindByName("*collection.*"));
dbCollections = All.GetByAncs(dbCollections.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
dbCollections.Add(All.FindAllReferences(dbCollections));

CxList dbCommands = dbCollections.GetMembersOfTarget();
dbCommands.Add(All.FindByName("*collection.*"));
dbCommands.Add(dbBasicCommands.GetMembersOfTarget());
dbCommands.Add(dbCommands.GetMembersOfTarget());
dbCommands.Add(dbCommands.GetMembersOfTarget());
dbCommands.Add(All.FindByShortName("order_by").GetMembersOfTarget());

List<string> dbCommandsList = new List<string> {
		"bulk_write", "save",
		"replace_one", "find_one*", "create_collection", "find_one_and*", 
		"find", "find_and_modify", "distinct", "group", "map_reduce", "runCommand",
		"findOne", "getCollection", "getSisterDB"
		};

result.Add(dbCommands.FindByShortNames(dbCommandsList));