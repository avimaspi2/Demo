/*
This query will contain all DB access through a user DAL.
It is empty by default, and will be filled when needed.
The query is used by the Find_DB query, and the various SQL_Injection
queries remove its result from the sink side of the path.
Notice: This DB access is assumed NOT to be susceptible to SQL Injection.
*/