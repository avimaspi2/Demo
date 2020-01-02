CxList methods = Find_Methods();

result = methods.FindByShortName("snprintf");
result.Add(methods.FindByShortName("_snprintf*"));
result.Add(methods.FindByShortName("_snwprintf*"));