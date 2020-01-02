CxList methods = Find_Methods();

result = methods.FindByMemberAccess("SforceService.Query", false);
result.Add(methods.FindByMemberAccess("SforceService.QueryAll", false));
result.Add(methods.FindByMemberAccess("SforceService.Search", false));