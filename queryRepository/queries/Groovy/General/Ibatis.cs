CxList methods = Find_Methods();
CxList ibatisSrc = 
	All.InheritsFrom("SqlMapClientOperations") + 
	All.InheritsFrom("SqlMapClientTemplate") +
	All.InheritsFrom("SqlMapClientImpl") +
	All.InheritsFrom("SqlMapClient");

CxList ibatisRef = All.FindAllReferences(ibatisSrc) - ibatisSrc;
CxList ibatisReff = ibatisRef.GetFathers();
CxList ibatis = ibatisReff.FindByType(typeof(MethodDecl)) + ibatisReff.FindByType(typeof(FieldDecl));
ibatis = All.FindAllReferences(ibatis);

CxList ibatisTypes = 
	All.FindByType("SqlMapClientOperations") + 
	All.FindByType("SqlMapClientTemplate") +
	All.FindByType("SqlMapClient") +
	All.FindByType("SqlMapClientImpl") +
	All.FindByShortName("getSqlMap");

CxList allIbatis = ibatisRef + ibatis + ibatisTypes;

CxList ibatisTarget = allIbatis.GetMembersOfTarget();
result =
	ibatisTarget.FindByShortName("queryForObject") + 
	ibatisTarget.FindByShortName("queryForList") + 
	ibatisTarget.FindByShortName("queryWithRowHandler") + 
	ibatisTarget.FindByShortName("queryForPaginatedList") + 
	ibatisTarget.FindByShortName("queryForMap") + 
	ibatisTarget.FindByShortName("insert") + 
	ibatisTarget.FindByShortName("update") + 
	ibatisTarget.FindByShortName("delete") +
	ibatisTarget.FindByShortName("execute") + 
	ibatisTarget.FindByShortName("executeWithListResult") + 
	ibatisTarget.FindByShortName("executeWithMapResult") +
	methods.FindByMemberAccess("SqlMapClientOperations.queryForObject") + 
	methods.FindByMemberAccess("SqlMapClientOperations.queryForList") + 
	methods.FindByMemberAccess("SqlMapClientOperations.queryWithRowHandler") + 
	methods.FindByMemberAccess("SqlMapClientOperations.queryForPaginatedList") + 
	methods.FindByMemberAccess("SqlMapClientOperations.queryForMap") + 
	methods.FindByMemberAccess("SqlMapClientOperations.insert") + 
	methods.FindByMemberAccess("SqlMapClientOperations.update") + 
	methods.FindByMemberAccess("SqlMapClientOperations.delete") +
	methods.FindByMemberAccess("SqlMapClientImpl.queryForObject") + 
	methods.FindByMemberAccess("SqlMapClientImpl.queryForList") + 
	methods.FindByMemberAccess("SqlMapClientImpl.queryWithRowHandler") + 
	methods.FindByMemberAccess("SqlMapClientImpl.queryForPaginatedList") + 
	methods.FindByMemberAccess("SqlMapClientImpl.queryForMap") + 
	methods.FindByMemberAccess("SqlMapClientImpl.insert") + 
	methods.FindByMemberAccess("SqlMapClientImpl.update") + 
	methods.FindByMemberAccess("SqlMapClientImpl.delete") +
	methods.FindByMemberAccess("SqlMapClient.queryForObject") + 
	methods.FindByMemberAccess("SqlMapClient.queryForList") + 
	methods.FindByMemberAccess("SqlMapClient.queryWithRowHandler") + 
	methods.FindByMemberAccess("SqlMapClient.queryForPaginatedList") + 
	methods.FindByMemberAccess("SqlMapClient.queryForMap") + 
	methods.FindByMemberAccess("SqlMapClient.insert") + 
	methods.FindByMemberAccess("SqlMapClient.update") + 
	methods.FindByMemberAccess("SqlMapClient.delete") +
	methods.FindByMemberAccess("SqlMapClientTemplate.queryForObject") + 
	methods.FindByMemberAccess("SqlMapClientTemplate.queryForList") + 
	methods.FindByMemberAccess("SqlMapClientTemplate.queryWithRowHandler") + 
	methods.FindByMemberAccess("SqlMapClientTemplate.queryForPaginatedList") + 
	methods.FindByMemberAccess("SqlMapClientTemplate.queryForMap") + 
	methods.FindByMemberAccess("SqlMapClientTemplate.insert") + 
	methods.FindByMemberAccess("SqlMapClientTemplate.update") + 
	methods.FindByMemberAccess("SqlMapClientTemplate.delete") +
	methods.FindByMemberAccess("SqlMapClientTemplate.execute") + 
	methods.FindByMemberAccess("SqlMapClientTemplate.executeWithListResult") + 
	methods.FindByMemberAccess("SqlMapClientTemplate.executeWithMapResult");