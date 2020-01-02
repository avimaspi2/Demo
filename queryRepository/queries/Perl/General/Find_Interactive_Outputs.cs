CxList methods = Find_Methods();

// console outputs
CxList outputs = 
	methods.FindByShortName("print") +
	methods.FindByShortName("printf") +
	methods.FindByShortName("echo") +
	methods.FindByShortName("say");

// Remove print commands with single parameter that is a string
CxList singleParam = outputs - outputs.FindByParameters(All.GetParameters(outputs, 1));
singleParam = All.GetParameters(singleParam);

singleParam = outputs.FindByParameters(singleParam.FindByType(typeof(StringLiteral)));
outputs -= singleParam;

// return statements in upper level - directly to an http page
CxList ns = All.FindByType(typeof(NamespaceDecl)).FindByShortName("Namespace_*");
CxList classes = All.FindByType(typeof(ClassDecl)).FindByShortName("Class_*").GetByAncs(ns);
CxList methodDecl = All.FindByType(typeof(MethodDecl));
CxList methods1 = methodDecl.FindByShortName("Method_*").GetByAncs(classes);
methods1.Add(methodDecl.FindByShortName("$").GetByAncs(classes));
methods1.Add(methodDecl.FindByShortName("").GetByAncs(classes));

CxList ret = All.FindByType(typeof(ReturnStmt)).GetByAncs(methods1);

result = outputs + All.FindByShortName("$*").GetByAncs(ret);