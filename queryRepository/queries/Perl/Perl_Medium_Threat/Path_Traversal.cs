CxList methods = Find_Methods();

// Find all open paramerters except for the 1st one
CxList open = methods.FindByShortName("open");
CxList openParams = All.GetParameters(open);
openParams -= openParams.GetParameters(open, 0);

CxList inputs = Find_Interactive_Inputs();

CxList sanitize = Find_General_Sanitize() + methods.FindByShortName("abs_path");

result = openParams.InfluencedByAndNotSanitized(inputs, sanitize);