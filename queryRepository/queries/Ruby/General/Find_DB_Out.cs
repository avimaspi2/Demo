CxList model = Find_Models();
if(model.Count > 0)
{
	CxList methods = Find_Methods();
	//old ActiveRecord inputs

	CxList classes = All.FindByType(typeof(ClassDecl));
	CxList active_record_class = classes.InheritsFrom("ActiveRecord.Base");
	CxList dbMethods = All.FindByType(active_record_class).GetMembersOfTarget();
	dbMethods.Add(All.FindAllReferences(active_record_class).GetMembersOfTarget());

	CxList finds = dbMethods.FindByShortName("find") +	
		methods.FindByShortName("find_by*") +
		methods.FindByShortName("find_first") +
		methods.FindByShortName("find_last") +
		methods.FindByShortName("find_one") +
		methods.FindByShortName("find_some") +	
		methods.FindByShortName("find_take") +
		methods.FindByShortName("find_with_associations") +	
		methods.FindByShortName("find_with_ids") +
		methods.FindByShortName("find_each") +
		methods.FindByShortName("find_in_batches") +
		methods.FindByShortName("all") +
		methods.FindByShortName("first") +
		methods.FindByShortName("first!") +
		methods.FindByShortName("last") +
		methods.FindByShortName("last!");

	// we need to look for also "self.class.find" when 
	// "self" refers to a class that inherits from ActiveRecord::Base
	CxList findMethods = methods.FindByShortName("find");
	foreach(CxList f in findMethods)
	{
		CxList target = f.GetTargetOfMembers().FindByShortName("class")
			.GetTargetOfMembers().FindByType(typeof(ThisRef));
		CxList def = All.FindDefinition(target);
		if (def.InheritsFrom("ActiveRecord.Base").Count > 0)
			finds.Add(f);
	}
	
	CxList memberAccess = All.FindByType(typeof(MemberAccess));

	memberAccess = memberAccess.FindByShortName("all") +
		memberAccess.FindByShortName("first") +
		memberAccess.FindByShortName("last");


	result.Add(finds + memberAccess);
}
result.Add(Find_DB_Out_DBA() +
	Find_DB_Out_DBX() +
	Find_DB_Out_MSSQL() +
	Find_DB_Out_MYSQL() +
	Find_DB_Out_ODBC() + 
	Find_DB_Out_ORACLE() +
	Find_DB_Out_PDO() +
	Find_DB_Out_PG());