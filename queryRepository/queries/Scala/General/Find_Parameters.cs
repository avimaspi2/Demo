//Finds all parametrized queries setters (setParameter setters)

CxList methods = Find_Methods();

CxList setPrms = 
	methods.FindByMemberAccess("PreparedStatement.set*") +
	methods.FindByMemberAccess("CallableStatement.set*"); 

CxList directPrepare = 
	methods.FindByMemberAccess("ConnectionWrapper.prepareStatement");

CxList ibatis = Ibatis();	// Ibatis
CxList qSQL1 = methods.FindByMemberAccess("QSqlQuery.bindValue");// QSql
CxList qSQL2 = methods.FindByMemberAccess("QSqlQuery.addBindValue");// QSql
CxList Prepared = methods.FindByMemberAccess("PreparedStatement.setString");

CxList getQ = Get_Query().GetMembersOfTarget();
CxList setters = getQ.FindByShortName("set*");
setters -= (getQ.FindByShortNames(new List<string> {
		"setCache*",
		"setComment",
		"setFetchSize",
		"setFirstResult",
		"setFlushMode",
		"setLock*",
		"setMaxResults",
		"setProperties",
		"setReadOnly",
		"setResultTransformer",
		"setSerialize*",
		"setTimeout"}));

CxList spring = methods.FindByMemberAccess("MapSqlParameterSource.addValue*");
CxList springMapSqlParameterSource = All.FindByType(typeof(ObjectCreateExpr)).FindByType("MapSqlParameterSource");

CxList springJdbcTemplate = All.NewCxList();
CxList JdbcTemplate = All.FindByMemberAccess("JdbcTemplate.query*");
CxList JdbcOperations = All.FindByMemberAccess("JdbcOperations.query*");

springJdbcTemplate.Add(JdbcTemplate.FindByMemberAccess("JdbcTemplate.query"));
springJdbcTemplate.Add(JdbcTemplate.FindByMemberAccess("JdbcTemplate.queryForInt"));
springJdbcTemplate.Add(JdbcTemplate.FindByMemberAccess("JdbcTemplate.queryForList"));
springJdbcTemplate.Add(JdbcTemplate.FindByMemberAccess("JdbcTemplate.queryForLong"));
springJdbcTemplate.Add(JdbcTemplate.FindByMemberAccess("JdbcTemplate.queryForMap"));
springJdbcTemplate.Add(JdbcTemplate.FindByMemberAccess("JdbcTemplate.queryForRowSet"));
springJdbcTemplate.Add(JdbcTemplate.FindByMemberAccess("JdbcTemplate.queryForObject"));
springJdbcTemplate.Add(All.FindByMemberAccess("JdbcTemplate.batchUpdate"));
springJdbcTemplate.Add(All.FindByMemberAccess("JdbcTemplate.update"));
springJdbcTemplate.Add(JdbcOperations.FindByMemberAccess("JdbcOperations.query"));
springJdbcTemplate.Add(JdbcOperations.FindByMemberAccess("JdbcOperations.queryForInt"));
springJdbcTemplate.Add(JdbcOperations.FindByMemberAccess("JdbcOperations.queryForList"));
springJdbcTemplate.Add(JdbcOperations.FindByMemberAccess("JdbcOperations.queryForLong"));
springJdbcTemplate.Add(JdbcOperations.FindByMemberAccess("JdbcOperations.queryForMap"));
springJdbcTemplate.Add(JdbcOperations.FindByMemberAccess("JdbcOperations.queryForRowSet"));
springJdbcTemplate.Add(JdbcOperations.FindByMemberAccess("JdbcOperations.queryForObject"));
springJdbcTemplate.Add(All.FindByMemberAccess("JdbcOperations.update"));
springJdbcTemplate.Add(All.FindByMemberAccess("JdbcOperations.batchUpdate"));

CxList prms = All.NewCxList();
prms.Add(All.GetParameters(spring));
prms.Add(All.GetParameters(setPrms));
prms.Add(All.GetParameters(ibatis, 1));
prms.Add(All.GetParameters(qSQL1, 1));
prms.Add(All.GetParameters(qSQL2, 0));
prms.Add(All.GetParameters(Prepared, 1));
prms.Add(All.GetParameters(setters, 1));
prms.Add(All.GetParameters(springMapSqlParameterSource, 1));
prms.Add(All.GetParameters(springJdbcTemplate));
prms -= All.GetParameters(springJdbcTemplate, 0);

CxList setParameters = All.GetParameters(methods.FindByMemberAccess(".setParameter"), 1);
CxList targetObjOfSetParams = setParameters.GetAncOfType(typeof(MethodInvokeExpr)).GetTargetOfMembers();
setParameters -= setParameters.GetParameters(targetObjOfSetParams.FindByType("*request").GetMembersOfTarget(), 1);
result = directPrepare.DataInfluencingOn(setPrms) + All.GetByAncs(prms) + setParameters;