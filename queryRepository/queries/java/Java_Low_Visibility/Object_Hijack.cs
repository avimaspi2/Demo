CxList cloneable = All.InheritsFrom("Cloneable");
CxList methodDecl = Find_MethodDecls();
CxList clone = methodDecl.GetByAncs(cloneable).FindByShortName("clone");

result = clone - clone.FindByFieldAttributes(Modifiers.Sealed);