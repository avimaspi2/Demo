// This query returns methods that retrieve data from the DataBase
string[] CoreDataMethodNames = {@"initWithFetchRequest:managedObjectContext:sectionNameKeyPath:cacheName:",
	@"executeFetchRequest:error:",
	@"objectRegisteredForID:",
	@"objectWithID:",
	@"existingObjectWithID:error:"
	};

// MagicalRecord is a wrapper for CoreData
string[] MRMethodNames = {
	@"MR_findAll",						// Retrieve objects
	@"MR_findAllSortedBy:ascending:",
	@"MR_findFirstByAttribute:withValue:",
	@"MR_findAllWithPredicate:",
	@"MR_requestAllWithPredicate:",
	@"MR_findFirstByAttribute:withValue:inContext:",
	@"MR_executeFetchRequest:",
	@"MR_aggregateOperation:onAttribute:withPredicate:",	// Aggregate reuslts
	@"MR_aggregateOperation:onAttribute:withPredicate:groupBy:",
	@"MR_numberOfEntities",				// Count results
	@"MR_numberOfEntitiesWithPredicate:",
	@"MR_countOfEntities",
	@"MR_countOfEntitiesWithContext:",
	@"MR_countOfEntitiesWithPredicate:",
	@"MR_countOfEntitiesWithPredicate:inContext:",
	@"MR_numberOfEntitiesWithContext:"
	};

CxList methods = Find_Methods();
List<string> methodNames = new List<string>(CoreDataMethodNames);
methodNames.AddRange(MRMethodNames);
result = methods.FindByShortNames(methodNames);

//Find all occurrences of registeredObjects property from NSManagedObjectContext
//Swift -> databaseOutputObject = moc?.registeredObjects
//ObjC -> databaseOutputObject = [moc registeredObjects]; 
result.Add(All.FindByMemberAccess("NSManagedObjectContext.registeredObjects"));

result -= result.FindByFileName("*.h"); // remove results from header files since they do not include the implementation