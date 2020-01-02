CxList methods = Find_Methods();

result = methods.FindByMemberAccess("SqlMapper.QueryForObject", false);
result.Add(methods.FindByMemberAccess("SqlMapper.QueryForList", false));
result.Add(methods.FindByMemberAccess("SqlMapper.QueryForPaginatedList", false));
result.Add(methods.FindByMemberAccess("SqlMapper.QueryForMap", false));
result.Add(methods.FindByMemberAccess("SqlMapper.Insert", false));
result.Add(methods.FindByMemberAccess("SqlMapper.Update", false));
result.Add(methods.FindByMemberAccess("SqlMapper.Delete", false));