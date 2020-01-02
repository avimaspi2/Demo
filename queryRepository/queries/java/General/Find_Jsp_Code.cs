result = All.FindByFileName("*.jsp");

// We treat Velocity templates as JSP
result.Add(All.FindByFileName("*.vm"));

CxList all = All - result;
all -= all.FindByFileName("*.java");

CxList JspClassInit = all.FindByShortName("Checkmarx_Class_Init");

CxList methodOfHttpServletRequest = all.FindByShortName("HttpServletRequest").GetAncOfType(typeof(MethodDecl));

JspClassInit = JspClassInit * methodOfHttpServletRequest;

CxList moreJsp = all.FindByType("object").FindByShortName("attr_StringByAction").FindByRegex(@"HttpServletRequest\srequest");

CxList allJsp = All.NewCxList();
allJsp.Add(JspClassInit);
allJsp.Add(moreJsp);

CxList JspNameSpace = allJsp.GetAncOfType(typeof(NamespaceDecl));

result.Add(all.GetByAncs(JspNameSpace));