//classes that inherits from Applet
CxList classDecl = Find_Class_Decl();

CxList classDeclApplet = All.NewCxList();
classDeclApplet.Add(classDecl.InheritsFrom("Applet"));
classDeclApplet.Add(classDecl.InheritsFrom("JApplet"));

CxList classAncs = All.GetByAncs(classDeclApplet);

// Find all method decl and method def
CxList methodDecl = classAncs.FindByType(typeof(MethodDecl));

// finalize methods
CxList finalize = methodDecl.FindByShortName("finalize");

result = finalize.FindByFieldAttributes(Modifiers.Public);