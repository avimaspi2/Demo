CxList methods = Find_Methods();
CxList createExp = All.FindByType(typeof(ObjectCreateExpr));
CxList memberAccess = All.FindByType(typeof(MemberAccess));

//Kohana_DB
CxList suspectedDbMethods = (methods + memberAccess).FindByShortName("query");
	
CxList dbInstance = 
	All.FindByMemberAccess("Database.instance") + 
	All.FindByMemberAccess("Database_MySQL.instance") + 
	All.FindByMemberAccess("Database_PDO.instance");

dbInstance.Add(createExp.FindByShortName("Database")); //Database constructor of Kohana 2.x versions

CxList dbInstanceInfluences = suspectedDbMethods.GetTargetOfMembers().DataInfluencedBy(dbInstance);	
CxList directDbMethods = (dbInstance + dbInstanceInfluences).GetMembersOfTarget() * suspectedDbMethods;

//Database member method query of a model class 
CxList modelDbAccess = All.FindByMemberAccess("_db.query"); 
CxList classes = All.FindByType(typeof(ClassDecl));
CxList dbModelClasses = classes.InheritsFrom("Model_Database") + classes.InheritsFrom("Kohana_Model_Database");
directDbMethods.Add(modelDbAccess.GetByAncs(dbModelClasses));

//query create from SQL statement which are executed later
CxList executeMethods = (methods + memberAccess).FindByShortName("execute"); 
CxList createQueryMethods =
	createExp.FindByShortName("Database_Query") +	
	createExp.FindByShortName("Kohana_Database_Query");
createQueryMethods.Add(All.FindByMemberAccess("DB.query"));//DB Kohana helper class static query method  
createQueryMethods = All.GetParameters(createQueryMethods, 1); //only the second parameter can recieve SQL statement

createQueryMethods.Add(All.FindByMemberAccess("DB.expr") + createExp.FindByShortName("Database_Expression"));
directDbMethods.Add(createQueryMethods * createQueryMethods.DataInfluencingOn(executeMethods));

//handling Kohana ORM 
CxList suspectedORM = (methods + memberAccess).FindByShortNames(new List<string>
	{"delete", "find", "find_all", "save","set"});
//	(methods + memberAccess).FindByShortName("delete") +
//	(methods + memberAccess).FindByShortName("find") +
//	(methods + memberAccess).FindByShortName("find_all") + 
//	(methods + memberAccess).FindByShortName("save") + 
//	(methods + memberAccess).FindByShortName("set");


CxList ORMinstance = All.FindByMemberAccess("ORM.factory");

//finds only the invokes of a Kohana ORM methods
CxList ORMinstanceInfluences = suspectedORM.GetTargetOfMembers().DataInfluencedBy(ORMinstance);	

CxList directORMmethods = (ORMinstance + ORMinstanceInfluences).GetMembersOfTarget() * suspectedORM;

result.Add(directDbMethods + directORMmethods);