CxList assert = 
	All.FindByMemberAccess("DBMS_ASSERT.ENQUOTE_LITERAL", false) + 
	All.FindByMemberAccess("DBMS_ASSERT.ENQUOTE_NAME", false) + 
	All.FindByMemberAccess("DBMS_ASSERT.SCHEMA_NAME", false) + 
	All.FindByMemberAccess("DBMS_ASSERT.SIMPLE_SQL_NAME", false) + 
	All.FindByMemberAccess("DBMS_ASSERT.SQL_OBJECT_NAME", false) + 
	All.FindByMemberAccess("DBMS_ASSERT.QUALIFIED_SQL_NAME", false);
	
CxList methods = Find_Methods();

CxList dateConversion = methods.FindByShortName("to_date");

result = dateConversion + assert + Find_Integers() + Find_Parameters();