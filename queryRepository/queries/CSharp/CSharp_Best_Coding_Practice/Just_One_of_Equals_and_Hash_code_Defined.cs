CxList methods = All.FindByType(typeof(MethodDecl));

CxList equals = methods.FindByShortName("Equals");
equals = equals.FindByFieldAttributes(Modifiers.Override);

CxList getHash = methods.FindByShortName("GetHashCode");
getHash = getHash.FindByFieldAttributes(Modifiers.Override);

CxList equalsClass = All.GetClass(equals);
CxList getHashClass = All.GetClass(getHash);

result.Add(equalsClass);
result.Add(getHashClass);
result -= equalsClass * getHashClass;