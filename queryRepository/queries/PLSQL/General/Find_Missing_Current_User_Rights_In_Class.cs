//packages & objects which do not have invoker rights (with AUTHID CURRENT_USER clause)
CxList packagesAndObjects = All.FindByType(typeof(ClassDecl));
CxList methodDecls = All.FindByType(typeof(MethodDecl));

CxList defaultClass = packagesAndObjects.FindByShortName("DefaultClass");
packagesAndObjects -= defaultClass;

CxList current_user = All.FindByCustomAttribute("current_user");
CxList packageObjectBody = methodDecls.GetAncOfType(typeof(ClassDecl)) - defaultClass;
result = packagesAndObjects - current_user.GetAncOfType(typeof(ClassDecl)) - packageObjectBody;