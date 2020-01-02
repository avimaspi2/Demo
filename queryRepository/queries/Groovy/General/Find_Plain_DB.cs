CxList methods = Find_Methods();
CxList queryCommands = Get_Query().GetMembersOfTarget();

CxList db = 
	methods.FindByMemberAccess("Statement.execute*") +
	methods.FindByMemberAccess("PreparedStatement.execute*") +
	methods.FindByMemberAccess("CallableStatement.execute*") + 
	methods.FindByMemberAccess("JdbcTemplate.query*") + 
	methods.FindByMemberAccess("JdbcTemplate.insert") + 
	methods.FindByMemberAccess("JdbcTemplate.update") + 
	methods.FindByMemberAccess("JdbcTemplate.delete") + 
	methods.FindByMemberAccess("JdbcTemplate.execute") +
	//methods.FindByMemberAccess("Session.createQuery") + // removed, we do not consider "create" as a DB
	//methods.FindByMemberAccess("Session.createSQLQuery") +
	//methods.FindByMemberAccess("Session.createFilter") +
	methods.FindByMemberAccess("SoapBindingStub.query") + 
	methods.FindByMemberAccess("SoapBindingStub.queryAll") + 
	methods.FindByMemberAccess("SoapBindingStub.search") + 
	// qSql
	methods.FindByMemberAccess("QSqlQuery.exec") +
	methods.FindByMemberAccess("QSqlQuery.execBatch") +
	// Query
	methods.FindByMemberAccess("Query.getSingleResult") +
	methods.FindByMemberAccess("Query.getResultList") +
	methods.FindByMemberAccess("Query.executeUpdate") +
	queryCommands.FindByShortName("getSingleResult") +
	queryCommands.FindByShortName("getResultList") +
	queryCommands.FindByShortName("executeUpdate") +
	// Salesforce 	
	methods.FindByMemberAccess("SforceService.query") + 
	methods.FindByMemberAccess("SforceService.queryAll") + 
	methods.FindByMemberAccess("SforceService.search") +
	//// public interface EntityManager // removed, we do not consider "create" as a DB
	//methods.FindByShortName("createQuery") + 
	//methods.FindByShortName("createNamedQuery") + 
	//methods.FindByShortName("createNativeQuery") +
	
	//Oracle DB out
	methods.FindByMemberAccess("CallableStatement.get*") +
	
	// spring Hibernate
	methods.FindByShortName("executeFind") +
	// Hibernate Query
	methods.FindByMemberAccess("Query.iterate") +
	methods.FindByMemberAccess("Query.list") +
	methods.FindByMemberAccess("Query.scroll") +
	methods.FindByMemberAccess("Query.uniqueResult") +
	queryCommands.FindByShortName("iterate") +
	queryCommands.FindByShortName("list") +
	queryCommands.FindByShortName("scroll") +
	queryCommands.FindByShortName("uniqueResult") + 
	
	// Groovy SQL Instance execution
	methods.FindByMemberAccess("Sql.newInstance") +
	methods.FindByMemberAccess("sql.execute*") + 
	methods.FindByMemberAccess("sql.eachRow") + 
	methods.FindByMemberAccess("sql.rows") + 
	methods.FindByMemberAccess("sql.runQuery");


CxList ibatis = Ibatis();

/* Hibernate */
CxList hibernate = Find_Hibernate_DB();
/* end Hibernate */

/* start JDO */
CxList newquery = All.FindByMemberAccess("PersistenceManager.newQuery");
CxList sqlType = All.FindByType(typeof(StringLiteral)).FindByName("*javax.jdo.query.SQL*");
sqlType.Add(All.FindByType(typeof(UnknownReference)).FindByShortName("Query").GetMembersOfTarget().FindByShortName("SQL"));
newquery = All.FindByParameters(sqlType) * newquery;
CxList sqlParam = All.GetParameters(newquery, 1);
CxList queryexecute = All.FindByMemberAccess("Query.execute");
CxList jdo = sqlParam.DataInfluencingOn(queryexecute) * sqlParam; //sql expression parameter which is inserted into query and executed later
/* end JDO*/

/*start SimpleDB*/
CxList obj = All.FindByType(typeof(ObjectCreateExpr)); 
CxList selectExp = All.GetByAncs(obj).FindByShortName("SelectRequest");
selectExp.Add(All.FindByMemberAccess("SelectRequest.setSelectExpression") + All.FindByMemberAccess("SelectRequest.withSelectExpression"));
CxList sdbc = All.FindByMemberAccess("AmazonSimpleDBClient.select");
CxList simpleDb = selectExp.DataInfluencingOn(sdbc) * selectExp;//SelectRequest which is inserted into select method (which executes the request)
/*end SimpleDB*/

// Add DAL_DB
CxList DAL = Find_DAL_DB();

result = db + ibatis + hibernate + DAL + jdo + simpleDb;

// Add GetAttribute fields that their setter is affected by DB
result.Add(Add_Get_Attribute(result));