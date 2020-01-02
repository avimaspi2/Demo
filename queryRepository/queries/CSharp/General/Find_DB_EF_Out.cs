CxList createExpr = All.FindByType(typeof(ObjectCreateExpr));
CxList typeRef = All.FindByType(typeof(TypeRef));
CxList methods = Find_Methods();

//////DbQuery and ObjectQuery
CxList objectQueryInherits = All.InheritsFrom("ObjectQuery");
objectQueryInherits.Add(All.InheritsFrom("DbQuery"));

CxList typeOfObjectQuery = All.FindByTypes(new string[]{"DbQuery","ObjectQuery"});
typeOfObjectQuery.Add(typeRef.FindByShortName(objectQueryInherits).GetFathers());

//Searches for
//ObjectQuery<Student> student = objctx.CreateQuery<Student>(sqlString);
CxList propertyRefs = typeOfObjectQuery.FindByType(typeof(Declarator));
propertyRefs.Add(typeOfObjectQuery.FindByType(typeof(UnknownReference)));

//Searches for
//Student std = objctx.CreateQuery<Student>(sqlString).FirstOrDefault<Student>();
propertyRefs.Add(methods.FindByShortName("CreateQuery").GetTargetOfMembers().GetMembersOfTarget());

//Searches for
// public global::System.Data.Objects.ObjectQuery<Car> Cars {
//    get {            
//          this._Cars = base.CreateQuery<Car>("[Cars]");               
//          return this._Cars;
//      }
// }
CxList propertyDeclaration = typeOfObjectQuery.GetFathers().FindByType(typeof(PropertyDecl));
propertyRefs.Add(All.FindAllReferences(propertyDeclaration));

// Find all DbQuery and ObjectQuery access
result = propertyRefs - propertyDeclaration - propertyRefs.FindByType(typeof(TypeRef));


////@model definitions
result.Add(createExpr.FindByShortName(Find_DB_EF_Models()));


////DbSet and ObjectSet
CxList dbSet = Find_DB_EF_DBSet();

result.Add(dbSet - dbSet.GetMembersOfTarget().GetTargetOfMembers());

result.Add(dbSet.GetMembersOfTarget().FindByShortNames(new List<string>{
		"Execute",
		"ExecuteAsync",
		"Find",
		"FindAsync",
		"SqlQuery",
		"FromSql",
		"ExecuteStoreCommand",
		"ExecuteStoreCommandAsync",
		"ExecuteStoreQuery",
		"ExecuteStoreQueryAsync",
		"ExecuteSqlCommand",
		"ExecuteSqlCommandAsync"
		}));

////ObjectContext and DbContext
CxList dbContextMethods = Find_DB_EF_DBContext().GetMembersOfTarget();
result.Add(dbContextMethods.FindByShortNames(new List<string>{
		"CreateDatabaseScript",
		"ExecuteFunction",
		"GetObjectByKey",		
		"SqlQuery",
		"FromSql",
		"ExecuteStoreCommand",
		"ExecuteStoreCommandAsync",
		"ExecuteStoreQuery",
		"ExecuteStoreQueryAsync",
		"ExecuteSqlCommand",
		"ExecuteSqlCommandAsync"
		}));

result.Add(All.GetParameters(dbContextMethods.FindByShortName("TryGetObjectByKey"), 1));