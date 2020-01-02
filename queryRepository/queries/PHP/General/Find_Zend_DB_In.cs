if (Find_Extend_Zend().Count > 0)
{
	CxList methods = Find_Methods();

	List < String > zendDbMethodsString = new List<String> {
			//Zend_Db_Adapter + methods which appear in other Zend classes
			"delete",
			"fetchAll", 
			"fetchAssoc", 
			"fetchCol", 
			"fetchOne", 
			"fetchPairs", 
			"fetchRow", 
			"insert",  
			"limit",  
			"query", 
			"prepare", 
			"update",
			//Zend_Db_Select
			"columns", 
			"from", 
			"group", 
			"having", 
			"join", 
			"joinCross", 
			"joinFull", 
			"joinInner", 
			"joinLeft", 
			"joinNatural", 
			"joinRight", 
			"orHaving", 
			"orWhere", 
			"union", 
			"where",
			//Zend_Db_Statement
			"execute",
			//Zend_Db_Table_Row + Zend_Db_Table_Rowset			
			"offsetGet",
			"offsetUnset",
			"offsetSet"};
/*
	//Zend_Db_Adapter + methods which appear in other Zend classes
	CxList directDbMethods = 
		methods.FindByShortName("delete") + 
		methods.FindByShortName("fetchAll") +
		methods.FindByShortName("fetchAssoc") +
		methods.FindByShortName("fetchCol") +
		methods.FindByShortName("fetchOne") +
		methods.FindByShortName("fetchPairs") +
		methods.FindByShortName("fetchRow") +
		methods.FindByShortName("insert") + 
		methods.FindByShortName("limit") + 
		methods.FindByShortName("query") +
		methods.FindByShortName("prepare") +
		methods.FindByShortName("update");


	//Zend_Db_Select
	directDbMethods.Add(
		methods.FindByShortName("columns") +
		methods.FindByShortName("from") +
		methods.FindByShortName("group") +
		methods.FindByShortName("having") +
		methods.FindByShortName("join") +
		methods.FindByShortName("joinCross") +
		methods.FindByShortName("joinFull") +
		methods.FindByShortName("joinInner") +
		methods.FindByShortName("joinLeft") +
		methods.FindByShortName("joinNatural") +
		methods.FindByShortName("joinRight") +
		methods.FindByShortName("orHaving") +
		methods.FindByShortName("orWhere") +
		methods.FindByShortName("union") +
		methods.FindByShortName("where"));

	//Zend_Db_Statement
	directDbMethods.Add(
		methods.FindByShortName("execute"));


	//Zend_Db_Table_Row + Zend_Db_Table_Rowset
	directDbMethods.Add(
		methods.FindByShortName("offsetGet") +
		methods.FindByShortName("offsetUnset") +
		methods.FindByShortName("offsetSet"));
*/
	CxList directDbMethods = methods.FindByShortNames(zendDbMethodsString);
	result.Add(directDbMethods);
}