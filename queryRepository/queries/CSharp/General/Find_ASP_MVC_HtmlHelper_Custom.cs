CxList methodsWithReturnTypes = All.NewCxList();

List < string > returnTypes = new List<string>(){"MvcHtmlString","HtmlString","IHtmlString"};

CxList allMethodDecl = All.FindByType(typeof(MethodDecl));

foreach(string type in returnTypes) {
	methodsWithReturnTypes.Add(allMethodDecl.FindByMethodReturnType(type));
}

CxList allMethodsWithTabBuilder = All.FindByName("TagBuilder").FindByType(typeof(TypeRef)).GetAncOfType(typeof(MethodDecl));

CxList methodWithMvcHtmlString = All.FindByName("MvcHtmlString").GetAncOfType(typeof(MethodDecl));

CxList allMethods = methodsWithReturnTypes + allMethodsWithTabBuilder + methodWithMvcHtmlString;

CxList allReferences = All.FindAllReferences(allMethods).FindByType(typeof(MethodInvokeExpr));

result = allReferences.FindByFileName("*.cshtml");