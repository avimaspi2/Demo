CxList methods = Find_Methods();
result = All.GetParameters(methods.FindByMemberAccess("WebServiceProxy.invoke"), 1);
result.Add(All.GetParameters(methods.FindByMemberAccess("Sys.CollectionChange"), 0));