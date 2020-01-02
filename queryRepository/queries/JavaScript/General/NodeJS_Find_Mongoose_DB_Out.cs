// Some DB_In methods return the document(s) writen in the DB, so they need to be in the DB_Out as well
CxList mongooseModels = NodeJS_Find_Mongoose_Models();
List <string> mongooseDbMethods = new List<string> {"find", "findOne","findById", "findOneAndUpdate", "findOneAndRemove",
		"findByIdAndDelete", "findByIdAndRemove", "findByIdAndUpdate", "findOneAndDelete", "insertMany", "populate", 
		"updateMany", "updateOne"};

CxList dbInvokes = mongooseModels.GetMembersOfTarget().FindByShortNames(mongooseDbMethods);

CxList lambdaExprs = Find_LambdaExpr();
CxList paramDecls = Find_ParamDecl();

result = paramDecls.GetParameters(lambdaExprs.GetParameters(dbInvokes), 1);

CxList queries = All.FindAllReferences(dbInvokes.GetAssignee());
dbInvokes.Add(queries);

CxList exec = dbInvokes.GetRightmostMember().FindByShortName("exec");
CxList execCallbackFunction = paramDecls.GetParameters(lambdaExprs.GetParameters(exec), 1);

result.Add(execCallbackFunction);