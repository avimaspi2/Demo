CxList methods = Find_Methods();
CxList queryCommands = Get_Query().GetMembersOfTarget();
CxList jdbc = methods.FindByMemberAccess("JdbcTemplate.*");
CxList query = methods.FindByMemberAccess("Query.*");
CxList db = methods.FindByMemberAccess("Statement.execute*");

// Prepare and Callable Statements
db.Add(All.FindByMemberAccess("PreparedStatement.execute*"));
db.Add(methods.FindByMemberAccess("CallableStatement.execute*"));

// JdbcTemplate methods
db.Add(jdbc.FindByMemberAccess("JdbcTemplate.query*"));
db.Add(jdbc.FindByMemberAccess("JdbcTemplate.insert"));
db.Add(jdbc.FindByMemberAccess("JdbcTemplate.update"));
db.Add(jdbc.FindByMemberAccess("JdbcTemplate.delete"));
db.Add(jdbc.FindByMemberAccess("JdbcTemplate.execute"));
db.Add(jdbc.FindByMemberAccess("JdbcTemplate.batchUpdate"));

// SoapBindingStub
db.Add(methods.FindByMemberAccess("SoapBindingStub.query"));
db.Add(methods.FindByMemberAccess("SoapBindingStub.queryAll"));
db.Add(methods.FindByMemberAccess("SoapBindingStub.search"));

// qSql
db.Add(methods.FindByMemberAccess("QSqlQuery.exec"));
db.Add(methods.FindByMemberAccess("QSqlQuery.execBatch"));

// Query
db.Add(query.FindByMemberAccess("Query.getSingleResult"));
db.Add(query.FindByMemberAccess("Query.getResultList"));
db.Add(query.FindByMemberAccess("Query.executeUpdate"));
db.Add(queryCommands.FindByShortNames(new List<string> {"getSingleResult", "getResultList", "executeUpdate"}));

// Salesforce 	
db.Add(methods.FindByMemberAccess("SforceService.query"));
db.Add(methods.FindByMemberAccess("SforceService.queryAll"));
db.Add(methods.FindByMemberAccess("SforceService.search"));

//Oracle DB out
db.Add(methods.FindByMemberAccess("Statement.get*"));
db.Add(methods.FindByMemberAccess("CallableStatement.get*"));
db.Add(methods.FindByMemberAccess("PreparedStatement.get*"));
	
// spring Hibernate
db.Add(methods.FindByShortName("executeFind"));

// Hibernate Query
db.Add(query.FindByMemberAccess("Query.iterate"));
db.Add(query.FindByMemberAccess("Query.list"));
db.Add(query.FindByMemberAccess("Query.scroll"));
db.Add(query.FindByMemberAccess("Query.uniqueResult"));
db.Add(queryCommands.FindByShortNames(new List<string> {"iterate","list","scroll","uniqueResult"}));

CxList ibatis = Ibatis();

/* MyBatis */
CxList myBatis = Find_MyBatis_DB();
	
/* Hibernate */
CxList hibernate = Find_Hibernate_DB();
/* end Hibernate */

/* start JDO */
CxList newquery = All.FindByMemberAccess("PersistenceManager.newQuery");
CxList sqlType = Find_Strings().FindByName("*javax.jdo.query.SQL*");
sqlType.Add(Find_UnknownReference().FindByShortName("Query").GetMembersOfTarget().FindByShortName("SQL"));
newquery = newquery.FindByParameters(sqlType);
CxList sqlParam = All.GetParameters(newquery, 1);
CxList queryexecute = All.FindByMemberAccess("Query.execute");
CxList jdo = sqlParam.DataInfluencingOn(queryexecute).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly); //sql expression parameter which is inserted into query and executed later
/* end JDO*/

/*start SimpleDB*/
CxList obj = All.FindByType(typeof(ObjectCreateExpr)); 
CxList selectExp = All.FindByShortName("SelectRequest").GetByAncs(obj);
selectExp.Add(All.FindByMemberAccess("SelectRequest.setSelectExpression") + All.FindByMemberAccess("SelectRequest.withSelectExpression"));
CxList sdbc = All.FindByMemberAccess("AmazonSimpleDBClient.select");
CxList simpleDb = selectExp.DataInfluencingOn(sdbc).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);//SelectRequest which is inserted into select method (which executes the request)
/*end SimpleDB*/

result = db;
result.Add(ibatis);
result.Add(myBatis);
result.Add(hibernate);
result.Add(jdo);
result.Add(simpleDb);