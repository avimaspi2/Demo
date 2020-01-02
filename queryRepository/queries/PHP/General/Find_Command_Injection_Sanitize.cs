CxList methods = Find_Methods();
result.Add(Find_General_Sanitize() - methods.FindByShortName("pcntl_exec"));
result.Add(Find_Encode());
result.Add(All.GetParameters(methods.FindByShortName("passthru"), 1));

result.Add(Find_HTML_Encode());