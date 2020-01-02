CxList methods = Find_Methods();
CxList con = All.FindByName("*getConnection") +	methods.FindByMemberAccess("Sql.newInstance");
CxList password = All.GetParameters(con, 2);

CxList inputs = Find_Read();
CxList sanitize = Find_General_Sanitize() + Find_Integers();

// All passwords of getConnection that are affected by a non-interactive input, and not well sanitized
result = password.InfluencedByAndNotSanitized(inputs, sanitize);