CxList methods = Find_Methods();

result = methods.FindByName("urllib.urlencode");
result.Add(methods.FindByName("urllib.quote"));
result.Add(methods.FindByName("urllib.quote_plus"));
	
	// version 3
result.Add(methods.FindByName("urllib.parse.urlencode"));
result.Add(methods.FindByName("urllib.parse.quote"));
result.Add(methods.FindByName("urllib.parse.quote_plus"));