CxList securedClasses = All.InheritsFrom("NSSecureCoding").FindByType(typeof(ClassDecl));
result = securedClasses;