CxList methods = Find_Methods();
CxList pwds = Find_Passwords();

CxList dbWrite = All.NewCxList();

CxList excludeElements = methods - Find_Param();
CxList nodeJsDbIn = All.GetParameters(NodeJS_Find_DB_IN()) - excludeElements;

// //- Add Mongoose DB write //-
List <string> mongooseDbWriteMethods = new List<string> {"update", "findOneAndUpdate", "findOneAndRemove", 
		"findByIdAndUpdate", "replaceOne", "updateMany", "updateOne"};

// Find all methods whose name is in "mongooseDbWriteMethods" and has as second parameter an element of "nodeJsDbIn"
CxList mongooseMethods = methods.FindByShortNames(mongooseDbWriteMethods);
//dbWrite.Add(methods.FindByParameters(nodeJsDbIn.GetParameters(methods.FindByShortNames(mongooseDbWriteMethods).FindByParameters(nodeJsDbIn), 1)));
dbWrite.Add(mongooseMethods.FindByParameters(nodeJsDbIn.GetParameters(mongooseMethods, 1)));
// //- x //-

// //- Add MongoDB DB write //-
List <string> mongoDbWriteMethods_0 = new List<string> {"insert", "insertOne", "insertMany"};
List <string> mongoDbWriteMethods_1 = new List<string> {"update", "updateOne", "updateMany", 
		"findOneAndUpdate", "findOneAndReplace", "replaceOne"};
List <string> mongoDbWriteMethods_2 = new List<string> {"findAndModify"};	

// Find all methods whose name is in "mongoDbWriteMethods_0" and has as first parameter an element of "nodeJsDbIn"
CxList mongoDbMethods_0 = methods.FindByShortNames(mongoDbWriteMethods_0);
dbWrite.Add(mongoDbMethods_0.FindByParameters(nodeJsDbIn.GetParameters(mongoDbMethods_0, 0)));

// Find all methods whose name is in "mongoDbWriteMethods_1" and has as second parameter an element of "nodeJsDbIn"
CxList mongoDbMethods_1 = methods.FindByShortNames(mongoDbWriteMethods_1);
dbWrite.Add(mongoDbMethods_1.FindByParameters(nodeJsDbIn.GetParameters(mongoDbMethods_1, 1)));

// Find all methods whose name is in "mongoDbWriteMethods_2" and has as third parameter an element of "nodeJsDbIn"
CxList mongoDbMethods_2 = methods.FindByShortNames(mongoDbWriteMethods_2);
dbWrite.Add(mongoDbMethods_2.FindByParameters(nodeJsDbIn.GetParameters(mongoDbMethods_2, 2)));

// Find all "bulkWrite" methods with elements of "nodeJsDbIn" as parameters 
dbWrite.Add(methods.FindByShortName("bulkWrite").FindByParameters(nodeJsDbIn));
// //-

// //- Add mysql DB write methods //-
List <string> mysqlDbWriteMethods = new List <string> {"query", "exec"};

// Find all methods whose name is in "mysqlDbWriteMethods" and has as second parameter an element of "nodeJsDbIn"
CxList mysqlMethods = methods.FindByShortNames(mysqlDbWriteMethods);
dbWrite.Add(mysqlMethods.FindByParameters(nodeJsDbIn.GetParameters(mysqlMethods, 1)));
// //- x //-

// //- Add nano DB write methods //-
List <string> nanoDbWriteMethods_0 = new List <string> {"insert"};
List <string> nanoDbWriteMethods_3 = new List <string> {"atomic"};

// Find all methods whose name is in "nanoDbWriteMethods_0" and has as first parameter an element of "nodeJsDbIn"
CxList nanoDbMethods_0 = methods.FindByShortNames(nanoDbWriteMethods_0);
dbWrite.Add(nanoDbMethods_0.FindByParameters(nodeJsDbIn.GetParameters(nanoDbMethods_0, 0)));

// Find all methods whose name is in "nanoDbWriteMethods_3" and has as fourth parameter an element of "nodeJsDbIn"
CxList nanoDbMethods_3 = methods.FindByShortNames(nanoDbWriteMethods_3);
dbWrite.Add(nanoDbMethods_3.FindByParameters(nodeJsDbIn.GetParameters(nanoDbMethods_3, 3)));
// //- x //-

// //- Add node-couchdb DB write methods //-
List <string> nodeCouchDbWriteMethods_1 = new List <string> {"insert", "update"};
List <string> nodeCouchDbWriteMethods = new List <string> {"insertAttachment", "updateFunction"};

// Find all methods whose name is in "nodeCouchDbWriteMethods_1" and has as first parameter an element of "nodeJsDbIn"
CxList nodeCouchDbMethods_Param_1 = methods.FindByShortNames(nodeCouchDbWriteMethods_1);
dbWrite.Add(nodeCouchDbMethods_Param_1.FindByParameters(nodeJsDbIn.GetParameters(nodeCouchDbMethods_Param_1, 1)));

// Find all methods whose name is in "nodeCouchDbWriteMethods" and has as parameters elements of "nodeJsDbIn"
CxList nodeCouchDbMethods_Param_All = methods.FindByShortNames(nodeCouchDbWriteMethods);
dbWrite.Add(nodeCouchDbMethods_Param_All.FindByParameters(nodeJsDbIn));
// //- x //-

// //- Add Redis DB write methods //-
List <string> redisDbWriteMethods = new List <string> {"hset", "hmset", "mset", "rpush", "sadd", "set"};

// Find all methods whose name is in "redisDbWriteMethods" and has as parameters elements of "nodeJsDbIn"
CxList redisDbMethods = methods.FindByShortNames(redisDbWriteMethods);
dbWrite.Add(redisDbMethods.FindByParameters(nodeJsDbIn));
// //- x //-

// //- Add sqlite DB write methods //-
List <string> sqliteDbWriteMethods = new List<string>{"get","all","each","exec"};

// Find all methods whose name is in "sqliteDbWriteMethods" and has as parameters elements of "nodeJsDbIn"
CxList sqliteDbMethods = methods.FindByShortNames(sqliteDbWriteMethods);
dbWrite.Add(sqliteDbMethods.FindByParameters(nodeJsDbIn));
// //- x //-

// //- Add Sequilize DB write methods //-
List <string> sequelizeDbWriteMethods = new List<string> {"build", "create", "findOrCreate", "bulkCreate", 
		"findCreateFind", "update", "upsert", "insertOrUpdate"};

// Find all methods whose name is in "sequelizeDbWriteMethods" and has as parameters elements of "nodeJsDbIn"
CxList sequelizeDbMethods = methods.FindByShortNames(sequelizeDbWriteMethods);
dbWrite.Add(sequelizeDbMethods.FindByParameters(nodeJsDbIn));
// //- x //-

// //- Add Cassandra DB write methods //-
List <string> cassandraDbWriteMethods = new List<string> {"execute", "batch", "stream"};

// Find all methods whose name is in "cassandraDbWriteMethods" and has as parameters elements of "nodeJsDbIn"
CxList cassandraDBMethods = methods.FindByShortNames(cassandraDbWriteMethods);
dbWrite.Add(cassandraDBMethods.FindByParameters(nodeJsDbIn));
// //- x //-

// //- Add couchbase DB write methods //-
List <string> couchbaseDbWriteMethods = new List<string> {"insert", "replace", "upsert", "query"};

// Find all methods whose name is in "couchbaseDbWriteMethods" and has as parameters elements of "nodeJsDbIn"
CxList couchbaseDBMethods = methods.FindByShortNames(couchbaseDbWriteMethods);
dbWrite.Add(couchbaseDBMethods.FindByParameters(nodeJsDbIn));
// //- x //-

CxList cryptoMembers = Find_Require("crypto");

CxList sanitize = cryptoMembers.GetAssignee().GetMembersOfTarget();
sanitize.Add(pwds.DataInfluencedBy(sanitize));

result = dbWrite.InfluencedByAndNotSanitized(pwds, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);