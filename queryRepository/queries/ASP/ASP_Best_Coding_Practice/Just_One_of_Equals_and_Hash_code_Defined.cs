CxList equals = All.FindByShortName("equals").FindByType(typeof(MethodDecl));
equals = equals.FindByFieldAttributes(Modifiers.Override);

CxList getHash = All.FindByShortName("gethashcode").FindByType(typeof(MethodDecl));
getHash = getHash.FindByFieldAttributes(Modifiers.Override);

result = All.GetClass(equals) - All.GetClass(equals) * All.GetClass(getHash) +
		 All.GetClass(getHash) - All.GetClass(equals) * All.GetClass(getHash);