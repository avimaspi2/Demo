CxList errorHandledPages = All.GetClass(All.FindByName("*page.errorpage"));
CxList AllPages = All.GetClass(All.FindByName("*page_load*").FindByType(typeof(MethodDecl)));

result = AllPages - errorHandledPages;