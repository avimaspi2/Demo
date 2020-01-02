CxList classes = Find_Class_Decl();
CxList servletClasses = classes.InheritsFrom("HttpServlet");
CxList singleThreadClasses = classes.InheritsFrom("SingleThreadModel");

// Find servlets that are multi-user (potential vulnerability)
CxList multiUserServlets = servletClasses - singleThreadClasses;

// Select only servlets that contain fields (thus can be used for passing data between users)
CxList classChildren;
CxList allClassFields;
CxList subClasses;
CxList subClassesFields;
CxList multiUserServletsAncs = All.GetByAncs(multiUserServlets);

foreach (CxList servlet in multiUserServlets)
{
	classChildren = multiUserServletsAncs.GetByAncs(servlet);
	allClassFields = classChildren * Find_Field_Decl();
	subClasses = classChildren.FindByType(typeof(ClassDecl)) - servlet;
	subClassesFields = allClassFields.GetByAncs(subClasses);
	CxList fields = allClassFields - subClassesFields;
	if (fields.Count > 0)
	{
		result.Add(fields.ConcatenateAllSources(servlet));
	}
}