// This query searches all  entities which are 
// mapped to DB tables (DbContext class) -> DbSet<TEntity> 
//
CxList classes = All.FindByType(typeof(ClassDecl));
CxList typeRef = All.FindByType(typeof(TypeRef));

CxList dbContext = All.InheritsFrom("DBContext");
dbContext.Add(All.InheritsFrom("ObjectContext"));

CxList dbSet = typeRef.FindByShortNames(new List<string>{"DbSet","ObjectSet"});//.GetByAncs(dbContext);

CxList propertyDecls = dbSet.GetFathers().FindByType(typeof(PropertyDecl));

List<string> names = new List<string>();
foreach(CxList dbSetProps in propertyDecls){
	try{
		PropertyDecl t = dbSetProps.data.GetByIndex(0) as PropertyDecl;
		string name = t.ShortName;
		if(name[name.Length - 1] == 's'){
			name = name.Substring(0, name.Length - 1);
			names.Add(name);
			names.Add(name + "Model");
		}
	} catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}
} 

result = classes.FindByShortNames(names);