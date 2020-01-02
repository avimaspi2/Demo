CxList outputs = NodeJS_Find_DB_IN();
CxList inputs = NodeJS_Find_Interactive_Inputs() + NodeJS_Find_Read();
CxList sanitize = NodeJS_Find_Sanitize();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);
result.Add(inputs * outputs - sanitize);