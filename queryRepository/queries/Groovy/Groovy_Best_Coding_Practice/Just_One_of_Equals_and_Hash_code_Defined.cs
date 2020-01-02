CxList methodDecl = All.FindByType(typeof(MethodDecl));

CxList Equals = methodDecl.FindByShortName("equals");
Equals = Equals.FindByFieldAttributes(Modifiers.Override);

CxList getHash = methodDecl.FindByShortName("hashCode");
getHash = getHash.FindByFieldAttributes(Modifiers.Override);

CxList equalsClass = All.GetClass(Equals);
CxList getHashClass = All.GetClass(getHash);

result.Add(equalsClass);
result.Add(getHashClass);
result -= equalsClass * getHashClass;