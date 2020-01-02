result = All.FindByFileName("*.jsf");
result.Add(All.FindByFileName("*.xhtml"));

CxList all = All - result;
all -= all.FindByFileName("*.java");

CxList JsfClassInit = all.FindByShortName("Checkmarx_Class_Init_JSF");

CxList methodOfHttpServletRequest = all.FindByShortName("HttpServletRequest").GetAncOfType(typeof(MethodDecl));

JsfClassInit = JsfClassInit * methodOfHttpServletRequest;

CxList JsfNameSpace = JsfClassInit.GetAncOfType(typeof(NamespaceDecl));

result.Add(all.GetByAncs(JsfNameSpace));