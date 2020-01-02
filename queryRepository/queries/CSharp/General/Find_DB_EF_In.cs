CxList ef = Find_DB_EF_DBContext();
ef.Add(Find_DB_EF_DBSet());

CxList dbDecl = ef.GetFathers().GetFathers().FindByType(typeof(Declarator));
CxList dbRef = All.FindAllReferences(dbDecl);

CxList createExpr = All.FindByType(typeof(ObjectCreateExpr));
CxList relevantCreateExpr = All.NewCxList();

foreach(CxList single in ef)
{
	CSharpGraph create = single.data.GetByIndex(0) as CSharpGraph;
	relevantCreateExpr.Add(createExpr.FindByType(create.FullName));
}

// dbVar below is for : db = new SampleDBEntities();
CxList dbVar = All.GetByAncs(relevantCreateExpr.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
dbRef.Add(All.FindAllReferences(dbVar));

//Find all DB access
CxList allEF = ef;
allEF.Add(dbRef);
allEF = allEF.GetMembersOfTarget();

// ObjectContext and DbContext
result = allEF.FindByShortNames(new List<string>{
		"Add",	
		"AddAsync",
		"AddRange",
		"AddRangeAsync",
		"DeleteDatabase",
		"Remove",
		"RemoveRange",
		"SaveChanges",
		"SaveChangesAsync",
		"Attach",
		"AttachRange",
		"Update",
		"UpdateRange"
		});

// ObjectContext and DbContext
// DbSet and ObjectSet
result.Add(All.GetParameters(allEF.FindByShortNames(new List<string>{
		//ObjectContext and DbContext
		"AddObject",
		"ExecuteFunction",
		"ExecuteStoreCommand",
		"ExecuteStoreCommandAsync",
		"ExecuteStoreQuery",
		"ExecuteStoreQueryAsync",
		"ExecuteSqlCommand",
		"ExecuteSqlCommandAsync",
		"GetObjectByKey",
		"DeleteObject",
		
		//DbSet and ObjectSet
		"Execute",
		"ExecuteAsync",
		"Find",
		"FindAsync",
		"SqlQuery",
		"FromSql"
		})));

// ObjectContext	
result.Add(All.GetParameters(allEF.FindByShortName("TryGetObjectByKey"), 0));