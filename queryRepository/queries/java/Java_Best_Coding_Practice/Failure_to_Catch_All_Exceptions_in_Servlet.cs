CxList servlets = Find_Class_Decl().InheritsFrom("HttpServlet");
CxList ServletsMethods = All.GetByAncs(servlets).FindByType(typeof(MethodDecl));

CxList Top_Level_Servlet_Methods = ServletsMethods.FindByShortNames(new List<string> {
		"doDelete",
		"doGet",
		"doHead",
		"doOptions",
		"doPost",
		"doPut",
		"doTrace",
		"service"}); 

// Finds the exceptions in the next line of the method signature	
CxList CxThrows = All.FindByCustomAttribute("CxThrows");
CxList IOException = All.FindByShortName("IOException");
CxList ServletException = All.FindByShortName("ServletException");

CxList sanitizers = All.NewCxList();
foreach (CxList throws in CxThrows)
{// All the methods that throw IOException and ServletException shouldn't be returned
	CxList t1 = IOException.GetByAncs(throws);
	CxList t2 = ServletException.GetByAncs(throws);
	if (t1.Count > 0 && t2.Count > 0)
		sanitizers.Add(throws);
}

result = Top_Level_Servlet_Methods - Top_Level_Servlet_Methods.GetMethod(sanitizers);