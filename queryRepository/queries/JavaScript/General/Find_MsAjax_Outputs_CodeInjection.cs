CxList methods = Find_Methods();
result = methods.FindByMemberAccess("_WebRequestManager.set_defaultExecutorType");
result.Add(All.GetParameters(methods.FindByMemberAccess("Type.parse"), 0));
result.Add(methods.FindByMemberAccess("Array.parse"));