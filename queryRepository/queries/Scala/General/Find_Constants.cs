result = All.FindByType(typeof(ConstantDecl));
result.Add(All.FindByType(typeof(ConstantDeclStmt)));
result.Add(All.FindByType(typeof(FieldDecl)).FindByFieldAttributes(Modifiers.Sealed));