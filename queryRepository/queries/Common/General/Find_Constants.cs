result = Find_ConstantDecl();
result.Add(Find_ConstantDeclStmt());
result.Add(Find_FieldDecls().FindByFieldAttributes(Modifiers.Sealed));