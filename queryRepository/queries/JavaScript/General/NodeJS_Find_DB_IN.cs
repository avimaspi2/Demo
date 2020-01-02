CxList onlyParams = Find_Parameters();
CxList uRef = Find_UnknownReference();
CxList mA = Find_MemberAccesses();
CxList urMA = uRef.Clone();
urMA.Add(mA);

CxList input = All.NewCxList();

////////////////////////////////////////////////////////////////////////////////////////////////////
//get all query() methods in files that include db-mysql or mysql DB (require('mysql'); or require('db-mysql');)
//input.Add(NodeJS_Find_DB_Base());
CxList queryAndExec = NodeJS_Find_DB_Base();

CxList _params = All.NewCxList();
_params.Add(uRef);
_params.Add(onlyParams);

CxList qeParams = _params.GetParameters(queryAndExec);
CxList qeAllParams = urMA.GetByAncs(qeParams);
//add params of query and execute mehtods
input.Add(qeParams);
//add y to inputs in case query or execute parameter is "xxx" + y + "zzz"...
input.Add(qeAllParams);
////////////////////////////////////////////////////////////////////////////////////////////////////
//input methods for nano, 
input.Add(NodeJS_DB_Input_Methods());
////////////////////////////////////////////////////////////////////////////////////////////////////
//Add mongoose
input.Add(NodeJS_Find_Mongoose_DB_In());
input.Add(NodeJS_Find_Sqlite_DB_In());
input.Add(NodeJS_Find_couchbase_DB_In());
//Add Redis
input.Add(NodeJS_Find_Redis_DB_In());
// Add Sequelize
input.Add(NodeJS_Find_Sequelize_DB_In());
 
result = input - XS_Find_All();