//Looks for the direct use of HttpRequests with arguments without using Properties such as QueryString, Cookies or Form
CxList methods = Find_Methods();
CxList problematic = methods.FindByShortNames(new List<string>(){"Request", "HttpRequest"}, false);
result = All.GetParameters(problematic, 0).FindByType(typeof(Param));