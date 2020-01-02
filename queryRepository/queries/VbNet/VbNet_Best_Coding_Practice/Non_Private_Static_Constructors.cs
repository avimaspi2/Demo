CxList nonPrivate = All.FindByFieldAttributes(Modifiers.Public | Modifiers.Protected);
CxList statics = All.FindByFieldAttributes(Modifiers.Static);
CxList ctors = All.FindByType(typeof(ConstructorDecl));

result = (statics * ctors) * nonPrivate;