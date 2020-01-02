CxList outputs = All.NewCxList();
//Add MongoDB
outputs.Add(NodeJS_MongoDB_Input_Methods());
//Add mongoose
outputs.Add(NodeJS_Find_Mongoose_DB_In());

// Add MarsDB
CxList marsDBIn = NodeJS_Find_MarsDB_DB_In();
outputs.Add(marsDBIn);
// Consider only MarsDB methods that allow sql manipulation, example: marsdbCollection.update({ "id": { "$ne": -1 }, "message": "NoSQL Injection!" })
List <string> ignoreMethods = new List<string> {"insertAll", "insert"};
CxList methodsToIgnore = marsDBIn.FindByShortNames(ignoreMethods);
marsDBIn = marsDBIn - methodsToIgnore;

CxList inputs = NodeJS_Find_Interactive_Inputs() + NodeJS_Find_Read();

CxList sanitize = NodeJS_Find_Sanitize(); 
// Consider only flows that contain the first parameter
CxList sanitizeParams = All.GetParameters(marsDBIn) - All.GetParameters(marsDBIn, 0);
sanitize.Add(sanitizeParams);

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);