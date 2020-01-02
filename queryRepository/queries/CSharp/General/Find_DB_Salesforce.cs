// Query(), QueryAll() and Search() for SforceService
CxList methods = Find_Methods();

result = methods.FindByMemberAccess("SforceService.Query");
result.Add(methods.FindByMemberAccess("SforceService.QueryAll"));
result.Add(methods.FindByMemberAccess("SforceService.Search"));