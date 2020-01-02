CxList methods = Find_Methods();

result = methods.FindByShortName("select") +
	methods.FindByMemberAccess("database.query") + 
//	methods.FindByMemberAccess("database.countquery") + 
	methods.FindByMemberAccess("search.query");