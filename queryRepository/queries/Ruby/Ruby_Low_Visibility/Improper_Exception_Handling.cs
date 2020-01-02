CxList allMethods = Find_Methods();

CxList ChildsOfSuppressWarning = All.GetByAncs(allMethods.FindByShortName("@"));
CxList ChildsOfTry = All.GetByAncs(All.FindByType(typeof(TryCatchFinallyStmt)));



// locate error related methods to filter out
CxList errMethods = 
	allMethods.FindByShortName("*error*") +
	allMethods.FindByShortName("*errno*") +
	allMethods.FindByShortName("sqlstate");

//locate predicates methods to filter out
CxList predicates = 
	allMethods.FindByShortName("exists?") +
	allMethods.FindByShortName("new_record?") +
	allMethods.FindByShortName("new?") +
	allMethods.FindByShortName("table_exists?") +
	allMethods.FindByShortName("column_exists?") +
	allMethods.FindByShortName("index_exists?") +
	allMethods.FindByShortName("index_name_exists?") +
	allMethods.FindByShortName("options_include_default?") +
	allMethods.FindByShortName("persisted?") +
	allMethods.FindByShortName("is_a?") +
	allMethods.FindByShortName("kind_of?") +
	allMethods.FindByShortName("include?") +
	allMethods.FindByShortName("instance_of?") +
	allMethods.FindByShortName("blank?") +
	allMethods.FindByShortName("nil?");

CxList dbMethods = Find_DB_Methods() - errMethods - predicates;

result = (dbMethods + Find_IO()) - ((ChildsOfTry + ChildsOfSuppressWarning) * (Find_DB_Methods() + Find_IO()));
result -= result.GetFathers();