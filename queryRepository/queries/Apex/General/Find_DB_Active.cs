CxList methods = Find_Methods();

result = 
	methods.FindByShortName("insert") +
	methods.FindByShortName("update") +
	methods.FindByShortName("delete") +
	methods.FindByShortName("merge") +
	methods.FindByShortName("upsert") +
	methods.FindByShortName("undelete") +
	methods.FindByShortName("convertlead");