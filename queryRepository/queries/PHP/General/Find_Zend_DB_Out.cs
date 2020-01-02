if (Find_Extend_Zend().Count > 0)
{
CxList methods = Find_Methods();

List < String > zendDbMethodsString = new List<String> {
	//Zend_Db_Adapter
	"describeTable",
	"fetchAll",
	"fetchAssoc",
	"fetchCol",
	"fetchOne",
	"fetchPairs",
	"fetchRow",
	"lastInsertId",
	"lastSequenceId",
	"nextSequenceId",
	"listTables",

	//Zend_Db_Table
	//"info",
		
	//Zend_Db_Table_Row + Zend_Db_Table_Rowset
	"offsetGet", 
	"fetchObject"};
/*
	//Zend_Db_Adapter
CxList directDbMethods = 
	methods.FindByShortName("describeTable") +
	methods.FindByShortName("fetchAll") +
	methods.FindByShortName("fetchAssoc") +
	methods.FindByShortName("fetchCol") +
	methods.FindByShortName("fetchOne") +
	methods.FindByShortName("fetchPairs") +
	methods.FindByShortName("fetchRow") +
	methods.FindByShortName("lastInsertId") +
	methods.FindByShortName("lastSequenceId") +	
	methods.FindByShortName("nextSequenceId") +
	methods.FindByShortName("listTables");


	//Zend_Db_Table
	//directDbMethods.Add(
	//	methods.FindByShortName("info"));

	//Zend_Db_Table_Row + Zend_Db_Table_Rowset
directDbMethods.Add(
	methods.FindByShortName("offsetGet") + 
	methods.FindByShortName("fetchObject"));
*/
CxList directDbMethods = methods.FindByShortNames(zendDbMethodsString);
result.Add(directDbMethods);
}