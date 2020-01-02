CxList methods = Find_Methods();
CxList queryCommands = Get_Query().GetMembersOfTarget();
CxList jdbc = methods.FindByMemberAccess("JdbcTemplate.*", false, StringComparison.Ordinal);
CxList query = methods.FindByMemberAccess("Query.*");

// Oracle DB out
CxList statment = All.FindByType("Statement");
statment.Add(All.FindByType("*.Statement"));
statment.Add(All.FindByType("PreparedStatement"));
statment.Add(All.FindByType("*.PreparedStatement"));
statment.Add(All.FindByType("CallableStatement"));
statment.Add(All.FindByType("*.CallableStatement"));
CxList statmentExecute = statment.GetMembersOfTarget().FindByShortNames(new List<string> {"execute*", "get*"});

CxList db = All.NewCxList();
db.Add(statmentExecute);

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

// Spring Hibernate
db.Add(methods.FindByShortName("executeFind"));

// Spring Query Annotation
CxList queryCustomAttr = Find_CustomAttribute().FindByName("Query");
CxList annotatedMethods = queryCustomAttr.GetAncOfType(typeof(MethodDecl));
db.Add(Find_ParamDeclaration().GetParameters(annotatedMethods));

// Hibernate Query
db.Add(query.FindByMemberAccess("Query.iterate"));
db.Add(query.FindByMemberAccess("Query.list"));
db.Add(query.FindByMemberAccess("Query.scroll"));
db.Add(query.FindByMemberAccess("Query.uniqueResult"));
db.Add(queryCommands.FindByShortNames(new List<string> {"iterate","list","scroll","uniqueResult"}));

db.Add(methods.FindByShortName("executeStatement"));

CxList ibatis = Ibatis();

// MyBatis
CxList myBatis = Find_MyBatis_DB();
	
// Hibernate 
CxList hibernate = Find_Hibernate_DB();

// JDO 
CxList newquery = All.FindByMemberAccess("PersistenceManager.newQuery");
CxList sqlType = Find_Strings().FindByName("*javax.jdo.query.SQL*");
sqlType.Add(Find_UnknownReference().FindByShortName("Query").GetMembersOfTarget().FindByShortName("SQL"));
newquery = newquery.FindByParameters(sqlType);
CxList sqlParam = All.GetParameters(newquery, 1);
CxList queryexecute = All.FindByMemberAccess("Query.execute");
CxList jdo = sqlParam.DataInfluencingOn(queryexecute).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly); //sql expression parameter which is inserted into query and executed later

// SimpleDB
CxList obj = Find_Object_Create(); 

CxList selectExp = All.FindByShortName("SelectRequest").GetByAncs(obj);
selectExp.Add(All.FindByMemberAccess("SelectRequest.setSelectExpression"));
selectExp.Add(All.FindByMemberAccess("SelectRequest.withSelectExpression"));

CxList sdbc = All.FindByMemberAccess("AmazonSimpleDBClient.select");
CxList simpleDb = selectExp.DataInfluencingOn(sdbc).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);//SelectRequest which is inserted into select method (which executes the request)

// Add DAL_DB
CxList DAL = Find_DAL_DB();

result = db;
result.Add(ibatis);
result.Add(myBatis);
result.Add(hibernate);
result.Add(DAL);
result.Add(jdo);
result.Add(simpleDb);

// PostgreSQL
result.Add(Find_PostgreSQL_DB_Base());