//Finds all parametrized queries setters (setParameter setters)

CxList methods = Find_Methods();

CxList setPrms = 
	methods.FindByMemberAccess("PreparedStatement.set*") +
	methods.FindByMemberAccess("CallableStatement.set*"); 

CxList ibatis = Ibatis();	// Ibatis
CxList qSQL1 = methods.FindByMemberAccess("QSqlQuery.bindValue");// QSql
CxList qSQL2 = methods.FindByMemberAccess("QSqlQuery.addBindValue");// QSql
CxList Prepared = methods.FindByMemberAccess("PreparedStatement.setString");

CxList getQ = Get_Query().GetMembersOfTarget();
CxList setters = getQ.FindByShortName("set*");
setters -= (getQ.FindByShortName("setCache*") +
	getQ.FindByShortName("setComment") +
	getQ.FindByShortName("setFetchSize") +
	getQ.FindByShortName("setFirstResult") +
	getQ.FindByShortName("setFlushMode") +
	getQ.FindByShortName("setLock*") +
	getQ.FindByShortName("setMaxResults") +
	getQ.FindByShortName("setProperties") +
	getQ.FindByShortName("setReadOnly") +
	getQ.FindByShortName("setResultTransformer") +
	getQ.FindByShortName("setSerialize*") +
	getQ.FindByShortName("setTimeout"));
	
/* processed in getQ = Get_Query()
CxList QueryImpl = methods.FindByMemberAccess("QueryImpl.setParameter"); // hibernate
CxList EJBQuery = methods.FindByMemberAccess("EJBQuery.setParameter");   // speedo
CxList JpaQuery = methods.FindByMemberAccess("JpaQuery.setParameter");   // cayenne
*/
CxList spring = methods.FindByMemberAccess("MapSqlParameterSource.addValue*");
CxList springMapSqlParameterSource = All.FindByType(typeof(ObjectCreateExpr)).FindByType("MapSqlParameterSource");

CxList springJdbcTemplate = 
	All.FindByMemberAccess("JdbcTemplate.query") +
	All.FindByMemberAccess("JdbcTemplate.queryForInt") +
	All.FindByMemberAccess("JdbcTemplate.queryForList") +
	All.FindByMemberAccess("JdbcTemplate.queryForLong") +
	All.FindByMemberAccess("JdbcTemplate.queryForMap") +
	All.FindByMemberAccess("JdbcTemplate.queryForRowSet") +
	All.FindByMemberAccess("JdbcTemplate.queryForObject") +
	All.FindByMemberAccess("JdbcTemplate.batchUpdate") +
	All.FindByMemberAccess("JdbcTemplate.update") +
	All.FindByMemberAccess("JdbcOperations.query") +
	All.FindByMemberAccess("JdbcOperations.queryForInt") +
	All.FindByMemberAccess("JdbcOperations.queryForList") +
	All.FindByMemberAccess("JdbcOperations.queryForLong") +
	All.FindByMemberAccess("JdbcOperations.queryForMap") +
	All.FindByMemberAccess("JdbcOperations.queryForRowSet") +
	All.FindByMemberAccess("JdbcOperations.queryForObject") +
	All.FindByMemberAccess("JdbcOperations.update") +
	All.FindByMemberAccess("JdbcOperations.batchUpdate");

CxList prms = 
	All.GetParameters(spring) +
	All.GetParameters(setPrms) +
	All.GetParameters(ibatis, 1) +
	All.GetParameters(qSQL1, 1) +
	All.GetParameters(qSQL2, 0) +
	All.GetParameters(Prepared, 1) +
	All.GetParameters(setters, 1) +
/*	included in setters
	All.GetParameters(QueryImpl, 1) +
	All.GetParameters(EJBQuery, 1) +
	All.GetParameters(JpaQuery, 1) + */
	All.GetParameters(springMapSqlParameterSource, 1) +
	All.GetParameters(springJdbcTemplate, 1);

result = setPrms + All.GetByAncs(prms) + All.GetParameters(methods.FindByMemberAccess(".setParameter"), 1);