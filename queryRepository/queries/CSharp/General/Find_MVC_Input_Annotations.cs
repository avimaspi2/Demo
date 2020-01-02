// Find all MVC methods with HTTP annotations and take their parameters

CxList inputAttributes = All.FindByType(typeof(CustomAttribute)).FindByShortNames(new List<string> {"HttpPost", "HttpPut", "HttpPatch", "HttpGet","HttpPostAttribute", "HttpPutAttribute", "HttpPatchAttribute", "HttpGetAttribute", "HttpHead", "HttpHeadAttribute", "HttpDelete", "HttpDeleteAttribute", "HttpOptions", "HttpOptionsAttribute","Route"});
CxList methods = inputAttributes.GetAncOfType(typeof(MethodDecl));
result = All.GetParameters(methods);