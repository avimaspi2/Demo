//// Find login operations using http instead of https

// Find all methods and strings
CxList methods = Find_Methods();
CxList strings = Find_Strings();

// Find login strings and http strings
CxList loginStrings = strings.FindByShortName("*/login*");
CxList httpStrings = strings.FindByShortName("*http:*");

// Find the "post" and its first parameter (where "http" and "login" are used)
CxList post = methods.FindByShortName("post");
CxList postParam = All.GetParameters(post, 0);

// Find all logins with http - either directly as a parameter, or affecting the parameter
CxList postLoginParam = postParam.DataInfluencedBy(loginStrings) + postParam * loginStrings;
postLoginParam = postLoginParam.DataInfluencedBy(httpStrings) + postLoginParam * httpStrings;


// Show the results as a flow from the parameter to post
result = postLoginParam.DataInfluencingOn(post);