CxList methodDecl = Find_MethodDeclaration();

CxList public_methods = methodDecl.FindByFieldAttributes(Modifiers.Public);
CxList remoteService = All.InheritsFrom("RemoteServiceServlet");
remoteService.Add(All.InheritsFrom("RpcServlet")); // EXPERIMENTAL and subject to change. Do not use this in production code
	
public_methods = public_methods.GetByAncs(remoteService);
result = public_methods;