CxList methods = Find_Methods();
CxList eval = methods.FindByShortName("eval");
CxList sanitizedEval = eval.FindByRegex("eval\\s+[{']");

CxList dynamic_method_invoke = eval;

dynamic_method_invoke.Add(methods.FindByShortName("evalbyte"));
dynamic_method_invoke.Add(methods.FindByShortName("qq"));

CxList inputs = Find_Interactive_Inputs();

CxList sanitize = Find_General_Sanitize();
sanitize.Add(sanitizedEval);

result = inputs.InfluencingOnAndNotSanitized(dynamic_method_invoke, sanitize);