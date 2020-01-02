CxList methods = Find_Methods();
CxList ibatisSrc = All.InheritsFrom("SqlMapClientOperations"); 
ibatisSrc.Add(All.InheritsFrom("SqlMapClientTemplate"));
ibatisSrc.Add(All.InheritsFrom("SqlMapClientImpl"));
ibatisSrc.Add(All.InheritsFrom("SqlMapClient"));

CxList ibatisRef = All.FindAllReferences(ibatisSrc) - ibatisSrc;
CxList ibatisReff = ibatisRef.GetFathers();
CxList ibatis = ibatisReff.FindByType(typeof(MethodDecl)) + (ibatisReff * Find_Field_Decl());
ibatis = All.FindAllReferences(ibatis);

CxList ibatisTypes = All.FindByTypes(new string [] {
	"SqlMapClientOperations",
	"SqlMapClientTemplate",
	"SqlMapClient",
	"SqlMapClientImpl"});

ibatisTypes.Add(methods.FindByShortNames(new List<string> {"getSqlMap", "getSqlMapClientTemplate"}));

CxList allIbatis = All.NewCxList();
allIbatis.Add(ibatisRef);
allIbatis.Add(ibatis);
allIbatis.Add(ibatisTypes);

CxList ibatisTarget = allIbatis.GetMembersOfTarget();

result = ibatisTarget.FindByShortNames(new List<string> {
		"queryForObject",
		"queryForList", 
		"queryWithRowHandler",
		"queryForPaginatedList",
		"queryForMap",
		"insert",
		"update",
		"delete",
		"execute",
		"executeWithListResult",
		"executeWithMapResult"});
result.Add(methods.FindByMemberAccess("SqlMapClientOperations.queryForObject"));
result.Add(methods.FindByMemberAccess("SqlMapClientOperations.queryForList"));
result.Add(methods.FindByMemberAccess("SqlMapClientOperations.queryWithRowHandler"));
result.Add(methods.FindByMemberAccess("SqlMapClientOperations.queryForPaginatedList"));
result.Add(methods.FindByMemberAccess("SqlMapClientOperations.queryForMap"));
result.Add(methods.FindByMemberAccess("SqlMapClientOperations.insert"));
result.Add(methods.FindByMemberAccess("SqlMapClientOperations.update"));
result.Add(methods.FindByMemberAccess("SqlMapClientOperations.delete"));
result.Add(methods.FindByMemberAccess("SqlMapClientImpl.queryForObject"));
result.Add(methods.FindByMemberAccess("SqlMapClientImpl.queryForList"));
result.Add(methods.FindByMemberAccess("SqlMapClientImpl.queryWithRowHandler"));
result.Add(methods.FindByMemberAccess("SqlMapClientImpl.queryForPaginatedList"));
result.Add(methods.FindByMemberAccess("SqlMapClientImpl.queryForMap"));
result.Add(methods.FindByMemberAccess("SqlMapClientImpl.insert"));
result.Add(methods.FindByMemberAccess("SqlMapClientImpl.update"));
result.Add(methods.FindByMemberAccess("SqlMapClientImpl.delete"));
result.Add(methods.FindByMemberAccess("SqlMapClient.queryForObject"));
result.Add(methods.FindByMemberAccess("SqlMapClient.queryForList"));
result.Add(methods.FindByMemberAccess("SqlMapClient.queryWithRowHandler"));
result.Add(methods.FindByMemberAccess("SqlMapClient.queryForPaginatedList"));
result.Add(methods.FindByMemberAccess("SqlMapClient.queryForMap"));
result.Add(methods.FindByMemberAccess("SqlMapClient.insert"));
result.Add(methods.FindByMemberAccess("SqlMapClient.update"));
result.Add(methods.FindByMemberAccess("SqlMapClient.delete"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.queryForObject"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.queryForList"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.queryWithRowHandler"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.queryForPaginatedList"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.queryForMap"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.insert"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.update"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.delete"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.execute"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.executeWithListResult"));
result.Add(methods.FindByMemberAccess("SqlMapClientTemplate.executeWithMapResult"));