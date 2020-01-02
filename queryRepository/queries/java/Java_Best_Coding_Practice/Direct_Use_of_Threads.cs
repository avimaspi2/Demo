CxList servletRequest = All.FindByType("HttpServletRequest");
CxList servletResponse = All.FindByType("HttpServletResponse");

CxList servlet1 = servletRequest.GetFathers().GetFathers().FindByType(typeof(MethodDecl));
CxList servlet2 = servletResponse.GetFathers().GetFathers().FindByType(typeof(MethodDecl));
CxList servlet = servlet1 * servlet2;

CxList runnable = All.FindByType("Runnable").FindByType(typeof(Declarator));

result = runnable.GetByAncs(servlet);

// Add "run" in thread
if(All.isWebApplication)
{
	CxList run = All.FindByShortName("run").FindByType(typeof(MethodDecl));
	result.Add(All.GetClass(run) * All.InheritsFrom("Thread"));
}