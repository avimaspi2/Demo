result = Find_Sanitize();
result.Add(Find_HTML_Encode());
result.Add(Find_SQL_Injection_Sanitize());
result.Add(Find_DB_Out());