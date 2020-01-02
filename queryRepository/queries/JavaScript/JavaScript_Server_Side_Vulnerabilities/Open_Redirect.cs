CxList outputs = NodeJS_Find_Redirect_outputs();
CxList inputs = NodeJS_Find_Interactive_Inputs();
CxList sanitize = NodeJS_Find_Redirect_Sanitize();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);
result.Add(inputs * outputs - sanitize);