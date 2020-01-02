CxList redirect = All.FindByMemberAccess("httpresponse.redirect") +
	All.FindByName("*response.redirect");

CxList inputs = All.FindByMemberAccess("*httprequest.QueryString_*") +
	All.FindByMemberAccess("*httprequest.QueryString.item") +
	All.FindByName("*request.Querystring_*") +
	All.FindByName("*request.QueryString.item") +
	All.FindByName("*request.item") +
	All.FindByShortName("request").FindByFathers(All.FindByType(typeof(IndexerRef)));

inputs -= inputs.DataInfluencingOn(inputs);	

CxList sanitize = Find_Integers();
result = redirect.InfluencedByAndNotSanitized(inputs, sanitize);