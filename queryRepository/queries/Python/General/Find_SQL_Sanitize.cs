result = Find_Sanitize();
result.Add(Find_Sanitize_Django_ORM());

// DB Sanitize
result.Add(Find_Sanitize_Postgres());