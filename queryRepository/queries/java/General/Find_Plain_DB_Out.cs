result = Find_DB_base();
result -= Find_Hibernate_DB().FindByShortNames(new List<string> {"createQuery", "createSQLQuery"});

// If it is an Android project - Add Android DB
if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_DB_Out());
}