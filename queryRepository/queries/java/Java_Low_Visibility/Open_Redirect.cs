CxList redirect = Find_Redirects();
CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_Redirect_Sanitizers();

result = redirect.InfluencedByAndNotSanitized(inputs, sanitize);