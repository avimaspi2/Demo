CxList redirect = All.FindByMemberAccess("HttpResponse.Redirect", false);
redirect.Add(All.FindByName("*Response.Redirect", false));

CxList inputs = All.FindByMemberAccess("*HttpRequest.QueryString_*", false);
inputs.Add(All.FindByMemberAccess("*HttpRequest.QueryString.item", false));
inputs.Add(All.FindByName("*Request.QueryString_*", false));
inputs.Add(All.FindByName("*Request.QueryString.Item", false));
inputs.Add(All.FindByName("*Request.Item", false));
inputs.Add(All.FindByShortName("Request", false).FindByFathers(All.FindByType(typeof(IndexerRef))));

inputs -= inputs.DataInfluencingOn(inputs);	

CxList sanitize = Find_Integers();

result = redirect.InfluencedByAndNotSanitized(inputs, sanitize);