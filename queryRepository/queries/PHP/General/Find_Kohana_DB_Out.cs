CxList methods = Find_Methods();
CxList createExp = All.FindByType(typeof(ObjectCreateExpr));
CxList memberAccess = All.FindByType(typeof(MemberAccess));

CxList methodAndMemberAccess = methods + memberAccess;

//Kohana_Database
CxList suspectedDbMethods = methodAndMemberAccess.FindByShortNames(new List<string> 
	{"query", "list_columns", "list_tables"});
//	(methods + memberAccess).FindByShortName("query") + 
//	(methods + memberAccess).FindByShortName("list_columns") + 
//	(methods + memberAccess).FindByShortName("list_tables");

CxList dbInstance = 
	All.FindByMemberAccess("Database.instance") + 
	All.FindByMemberAccess("Database_MySQL.instance") + 
	All.FindByMemberAccess("Database_PDO.instance");

dbInstance.Add(createExp.FindByShortName("Database"));

//finds only the invokes of a Kohana Database
CxList dbInstanceInfluences = suspectedDbMethods.GetTargetOfMembers().DataInfluencedBy(dbInstance);	
CxList directDbMethods = (dbInstance + dbInstanceInfluences).GetMembersOfTarget() * suspectedDbMethods;

//Database member method query of a model class 
CxList modelDbAccess = 
	All.FindByMemberAccess("_db.query") + 
	All.FindByMemberAccess("_db.list_columns") + 
	All.FindByMemberAccess("_db.list_tables");
CxList classes = All.FindByType(typeof(ClassDecl));
CxList dbModelClasses = classes.InheritsFrom("Model_Database") + classes.InheritsFrom("Kohana_Model_Database");
directDbMethods.Add(modelDbAccess.GetByAncs(dbModelClasses));

//handling Kohana Result
CxList suspectedResult = methodAndMemberAccess.FindByShortNames(new List<string>
	{"as_array", "cached", "get","offsetGet"});
//	(methods + memberAccess).FindByShortName("as_array") +
//	(methods + memberAccess).FindByShortName("cached") +
//	(methods + memberAccess).FindByShortName("get") +
//	(methods + memberAccess).FindByShortName("offsetGet");


CxList Resultinstance = createExp.FindByShortNames(new List<string>
	{"Database_Result", "Database_MySQL_Result", "Database_Result_Cached"});
//	createExp.FindByShortName("Database_Result") +
//	createExp.FindByShortName("Database_MySQL_Result") +
//	createExp.FindByShortName("Database_Result_Cached");

Resultinstance.Add(methodAndMemberAccess.FindByShortName("execute"));

//finds only the invokes of a Kohana ORM methods
CxList ResultInstanceInfluences = suspectedResult.GetTargetOfMembers().DataInfluencedBy(Resultinstance);	
CxList directResultmethods = (Resultinstance + ResultInstanceInfluences).GetMembersOfTarget() * suspectedResult;

//handling Kohana ORM 
CxList suspectedORM = methodAndMemberAccess.FindByShortNames(new List<string>
	{"as_array", "find", "find_all","list_columns", "save"});
//	(methods + memberAccess).FindByShortName("as_array") +
//	(methods + memberAccess).FindByShortName("find") +
//	(methods + memberAccess).FindByShortName("find_all") + 
//	(methods + memberAccess).FindByShortName("list_columns") +
//	(methods + memberAccess).FindByShortName("save");

CxList ORMinstance = All.FindByMemberAccess("ORM.factory");
ORMinstance.Add(createExp.FindByShortName("Model_*"));

//finds only the invokes of a Kohana ORM methods

CxList ORMinstanceInfluences = suspectedORM.GetTargetOfMembers().DataInfluencedBy(ORMinstance);	
CxList directORMmethods = (ORMinstance + ORMinstanceInfluences).GetMembersOfTarget() * suspectedORM;

result.Add(directDbMethods + directORMmethods + directResultmethods);