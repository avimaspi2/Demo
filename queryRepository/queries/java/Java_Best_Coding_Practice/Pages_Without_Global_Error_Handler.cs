CxList errorHandledPages = All.GetClass(All.FindByName("*page.errorPage"));
CxList AllPages = All.GetClass(All.FindByName("*Checkmarx_Class_Init*").FindByType(typeof(MethodDecl)));

result = AllPages - errorHandledPages;