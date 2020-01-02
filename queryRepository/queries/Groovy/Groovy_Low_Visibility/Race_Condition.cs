// Race_Condition query 
// Returns all Singleton classes fields references with value assignment

CxList classes = Find_Class_Decl();

// Find all classes that are HTTP Servlets and Singleton 
CxList servletClasses = All.NewCxList();
CxList singleLevel = classes.InheritsFrom("HttpServlet") 
	+ classes.InheritsFrom("HttpJspBase");
int levelCount = singleLevel.Count;
int counter = 0;
while (levelCount > 0 && counter++ < 100)
{
	servletClasses.Add(singleLevel);
	singleLevel = classes.InheritsFrom(singleLevel);
	levelCount = singleLevel.Count;
}

// Find all classes that implement a single-thread model
CxList singleThreadClasses = All.NewCxList();
singleLevel = classes.InheritsFrom("SingleThreadModel");
levelCount = singleLevel.Count;
counter = 0;
while (levelCount > 0 && counter++ < 100)
{
	singleThreadClasses.Add(singleLevel);
	singleLevel = classes.InheritsFrom(singleLevel);
	levelCount = singleLevel.Count;
}

// Find servlets that are multi-user (potential vulnerability)
CxList multiUserServlets = servletClasses - singleThreadClasses;

// Select only servlets that contain fields (thus can be used for passing data between users)
CxList classChildren;
CxList allClassFields;
CxList subClasses;
CxList subClassesFields;
CxList allFields = All.NewCxList();
foreach (CxList servlet in multiUserServlets)
{
	classChildren = All.GetByAncs(servlet);
	allClassFields = classChildren.FindByType(typeof(FieldDecl));
	subClasses = classChildren.FindByType(typeof(ClassDecl)) - servlet;
	subClassesFields = allClassFields.GetByAncs(subClasses);
	CxList fields = allClassFields - subClassesFields;
	if (fields.Count > 0)
	{
		allFields.Add(fields.ConcatenateAllSources(servlet));
	}
}

// Find All HTTP Servlets classes methods
CxList methods = All.GetByAncs(multiUserServlets).FindByType(typeof(MethodDecl));
CxList syncBlocks = All.GetByAncs(methods).FindByType(typeof(LockStmt));		//synchronized blocks
CxList syncMethods = All.FindByCustomAttribute("synchronized").GetFathers();	//synchronized methods
//volatile variables
CxList allUnkownRefs = All.FindByType(typeof(UnknownReference));
CxList allIndexers = All.FindByType(typeof(IndexerRef));

//unknown referenses in constructors
CxList allCostructorVars = allUnkownRefs.GetByAncs(All.FindByType(typeof (ConstructorDecl)));
CxList allRefs = allUnkownRefs + allIndexers - allUnkownRefs.GetByAncs(allIndexers) - allCostructorVars;

CxList volatileVar = All.FindByFieldAttributes(Modifiers.Volatile);
volatileVar = allRefs.FindAllReferences(volatileVar);
CxList allSyncVars = allRefs.GetByAncs(syncBlocks) + allRefs.GetByAncs(syncMethods) + volatileVar;

//Find all assignments to class fields references
CxList fieldsReferences = allRefs.FindAllReferences(allFields) - allSyncVars;
CxList fieldsRefFathers = fieldsReferences.GetFathers().FindByType(typeof (AssignExpr));

result = allRefs.GetByAncs(fieldsRefFathers) * fieldsReferences;