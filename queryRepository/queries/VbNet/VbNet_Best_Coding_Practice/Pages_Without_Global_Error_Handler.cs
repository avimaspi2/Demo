CxList errorHandledPages = All.GetClass(All.FindByName("*Page.ErrorPage", false));
CxList AllPages = All.GetClass(All.FindByName("*Page_Load*", false).FindByType(typeof(MethodDecl)));

result = AllPages - errorHandledPages;