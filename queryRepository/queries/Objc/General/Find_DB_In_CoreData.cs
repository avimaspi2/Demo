CxList methods = Find_Methods();
CxList newInsertedObjects = methods.FindByShortNames(new List<string> 
	{@"insertNewObjectForEntityForName:inManagedObjectContext:", @"insertNewObject:into:"});
newInsertedObjects.Add(newInsertedObjects.GetAssignee());
CxList insertedObjects = All.FindAllReferences(newInsertedObjects).GetMembersOfTarget();
//Find cases where regular NSManagedObjects are used
CxList directAccess = insertedObjects.FindByShortNames(new List<string>{@"setValue:forKey:", @"setValue:forKeyPath:"});
result.Add(All.GetParameters(directAccess, 0));

	//Find cases where NSManagedObject is extended
	insertedObjects -= directAccess;
CxList inDirectAccess = insertedObjects.FindByType(typeof(MethodInvokeExpr));

//If dot notation stays memberaccess change accordingly - add assign case.

foreach(CxList obj in inDirectAccess) 
{
	try 
	{
		MethodInvokeExpr m = obj.TryGetCSharpGraph<MethodInvokeExpr>();
		if (m.Parameters.Count == 1) 
		{
			result.Add(obj);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

// MagicalRecord is a wrapper for CoreData
string[] MRMethodNames = {
	@"MR_createEntity",				// Create objects
	@"MR_createEntityInContext:",
	@"MR_createInContext:",
	@"MR_deleteEntity",					// Delete object
	@"MR_deleteEntityInContext:",
	@"MR_deleteInContext:",
	@"MR_truncateAll",
	@"MR_truncateAllInContext:"
	};
result.Add(methods.FindByShortNames(new List<string>(MRMethodNames)));