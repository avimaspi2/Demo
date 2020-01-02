//This queries searches for data being sent to a MongoDb through mongoose

CxList mongooseModels = NodeJS_Find_Mongoose_Models();

List <string> mongooseDbInMethods = new List<string> {"find", "findOne","findById", "update", "findOneAndUpdate", "findOneAndRemove",
		"remove", "bulkWrite", "deleteOne", "deleteMany", "findByIdAndDelete", "findByIdAndRemove",
		"findByIdAndUpdate", "findOneAndDelete", "insertMany", "populate", "replaceOne", "updateMany", "updateOne"};

List <string> mongooseDBWriteMethods = new List<string> {"update", "findOneAndUpdate", "findOneAndRemove", "findByIdAndUpdate", 
		"replaceOne", "updateMany", "updateOne"};
	
result = All.GetParameters(mongooseModels.GetMembersOfTarget().FindByShortNames(mongooseDbInMethods), 0);
result.Add(All.GetParameters(mongooseModels.GetMembersOfTarget().FindByShortNames(mongooseDBWriteMethods), 1));