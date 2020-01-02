CxList nonPrivate = All.FindByFieldAttributes(Modifiers.Public | Modifiers.Protected);
CxList statics = All.FindByFieldAttributes(Modifiers.Static);
CxList ctors = Find_ConstructorDecl();

result = (statics * ctors) * nonPrivate;