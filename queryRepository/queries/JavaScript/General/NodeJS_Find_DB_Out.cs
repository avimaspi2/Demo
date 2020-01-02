CxList allMethDecl = Find_MethodDecls();
CxList _params = Find_Parameters();
CxList allPD = Find_ParamDecl();
CxList uRef = Find_UnknownReference();
CxList mA = Find_MemberAccesses(); 
CxList allMethInv = Find_Methods();
CxList urMA = uRef.Clone();
urMA.Add(mA);
CxList callbacks = Find_LambdaExpr();

CxList output = All.NewCxList();

////////////////////////////////////////////////////////////////////////////////////////////////////
//get all query() methods in files that include db-mysql or mysql DB (require('mysql'); or require('db-mysql');)
CxList fromBase = NodeJS_Find_DB_Base();
CxList fromBaseNotMethods = fromBase - allMethInv * fromBase;	//remove all methods
output.Add(fromBaseNotMethods);
////////////////////////////////////////////////////////////////////////////////////////////////////
//Add all parameters of output methods to output
_params.Add(uRef);
CxList outField = _params.GetParameters(fromBase);
outField = urMA.GetByAncs(outField);

//Get All parameters of query's or execute's callback function as MethodDecl
CxList outFieldAsMD = callbacks.GetByAncs(fromBase);
outFieldAsMD.Add(allMethDecl.FindDefinition(outField));

//the output is all parameter of callbadk method, but first parameter
CxList notFirstParam = allPD.GetParameters(outFieldAsMD, 1);
output.Add(notFirstParam);
////////////////////////////////////////////////////////////////////////////////////////////////////

//output methods for nano, node-couchdb and mongodb
output.Add(NodeJS_DB_Output_Methods());

//Add mongoose
output.Add(NodeJS_Find_Mongoose_DB_Out());
// Add Sequelize
output.Add(NodeJS_Find_Sequelize_DB_Out());
// Add Sqlite
output.Add(NodeJS_Find_Sqlite_DB_Out());
 
result = output - XS_Find_All();