// Find controllers
CxList controllerInstances = All.FindByShortNames(new List<string>{"ApiController", "Controller"});
CxList classDecls = controllerInstances.GetFathers().GetFathers();

classDecls.Add(All.InheritsFrom(classDecls));
classDecls = classDecls.FindByType(typeof(ClassDecl));

// Find all methods of controllers
CxList methodDecls = All.FindByType(typeof(MethodDecl)).GetByClass(classDecls);

// Keep only public methods
methodDecls = methodDecls.FindByFieldAttributes(Modifiers.Public);

// Remove methods tagged with the NonAction attribute
CxList nonActionMethods = All.FindByCustomAttribute("NonAction").GetByAncs(methodDecls).GetAncOfType(typeof(MethodDecl));
methodDecls -= nonActionMethods;

result.Add(methodDecls);