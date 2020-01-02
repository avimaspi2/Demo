CxList cloneable = All.InheritsFrom("Cloneable");
CxList methodDecl = All.FindByType(typeof(MethodDecl));
CxList clone = methodDecl.GetByAncs(cloneable).FindByShortName("clone");

result = clone - clone.FindByFieldAttributes(Modifiers.Sealed);