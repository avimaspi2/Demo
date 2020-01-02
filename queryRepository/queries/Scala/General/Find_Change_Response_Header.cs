CxList methods = All.FindByType(typeof(MethodInvokeExpr));
CxList methodsChangeHeaders = methods.FindByShortNames(new List<string>{"add","setHeader","withHeaders"});
//Dom doesn't link methods to the classes being used thus the search needs to be done by method
CxList putHeaders = methods.FindByShortName("putHeaders");
CxList headerPutHeaders = methods.GetParameters(putHeaders).FindByShortName("Header");
methodsChangeHeaders.Add(headerPutHeaders);
result = methodsChangeHeaders;