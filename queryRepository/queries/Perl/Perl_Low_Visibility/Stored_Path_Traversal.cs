CxList methods = Find_Methods();

// Find all open paramerters except for the 1st one
CxList open = methods.FindByShortName("open");
CxList openParams = All.GetParameters(open);
openParams -= openParams.GetParameters(open, 0);

CxList inputs = Find_Read() + Find_DB_Out();

CxList sanitize = Find_General_Sanitize() + methods.FindByShortName("abs_path");

result = open.InfluencedByAndNotSanitized(inputs, sanitize);