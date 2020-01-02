result = Find_DB_base();
result.Add(Find_DB_In_JdbcTemplate());
result.Add(Find_PostgreSQL_DB_In());

// If it is an Android project - Add Android DB
if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_DB_In());
}