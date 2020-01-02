// Uses the Object context to get (Object/DB)Sets property access

result = Find_DB_EF_DBContext().GetMembersOfTarget().FindByType(typeof(MemberAccess));
result -= result.FindByShortNames(new List<string>{
			// from DBContext
			"ChangeTracker","Configuration","ContextType", "Extensions",//"Database",
			// from ObjectContext
			"CommandTimeout","Connection","ContextOptions",
			"DefaultContainerName","MetadataWorkspace",
			"ObjectStateManager","QueryProvider", 
			"InterceptionContext","TransactionHandler"});