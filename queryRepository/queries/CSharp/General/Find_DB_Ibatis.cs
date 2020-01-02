//QueryForObject(), QueryForList(), QueryForMap(), Insert(), Update() and
//Delete() methods for SqlMapper
CxList methods = Find_Methods();
CxList temp = methods.FindByMemberAccess("SqlMapper.*");

result = temp.FindByMemberAccess("SqlMapper.QueryForObject");
result.Add(temp.FindByMemberAccess("SqlMapper.QueryForList"));
result.Add(temp.FindByMemberAccess("SqlMapper.QueryForPaginatedList"));
result.Add(temp.FindByMemberAccess("SqlMapper.QueryForMap"));
result.Add(temp.FindByMemberAccess("SqlMapper.Insert"));
result.Add(temp.FindByMemberAccess("SqlMapper.Update"));
result.Add(temp.FindByMemberAccess("SqlMapper.Delete"));