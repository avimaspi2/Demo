CxList equals = All.FindByShortName("Equals", false).FindByType(typeof(MethodDecl));
equals = equals.FindByFieldAttributes(Modifiers.Override);

CxList getHash = All.FindByShortName("GetHashCode", false).FindByType(typeof(MethodDecl));
getHash = getHash.FindByFieldAttributes(Modifiers.Override);

CxList equalsClass  = All.GetClass(equals);
CxList getHashClass = All.GetClass(getHash);
CxList intersectEqualsAndGetHash = equalsClass * getHashClass;	

result = equalsClass - intersectEqualsAndGetHash;
result.Add(getHashClass - intersectEqualsAndGetHash);