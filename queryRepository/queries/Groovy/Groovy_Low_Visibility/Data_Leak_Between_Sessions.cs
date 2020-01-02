CxList classes = Find_Class_Decl();

// Find all classes that are HTTP Servlets
CxList servletClasses = All.NewCxList();
CxList singleLevel = classes.InheritsFrom("HttpServlet");
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
foreach (CxList servlet in multiUserServlets)
{
	classChildren = All.GetByAncs(servlet);
	allClassFields = classChildren.FindByType(typeof(FieldDecl));
	subClasses = classChildren.FindByType(typeof(ClassDecl)) - servlet;
	subClassesFields = allClassFields.GetByAncs(subClasses);
	CxList fields = allClassFields - subClassesFields;
	if (fields.Count > 0)
	{
		result.Add(fields.ConcatenateAllSources(servlet));
	}
}