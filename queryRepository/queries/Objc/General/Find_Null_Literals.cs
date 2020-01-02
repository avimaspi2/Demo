CxList nullLiterals = Find_NullLiteral();
nullLiterals.Add(All.FindByShortNames(new List<string>{"null", "nsnull", "nil"}));
result.Add(nullLiterals.FindByRegex("(null|NULL|Null|nil|Nil)"));

CxList memberAccess = Find_MemberAccess();
CxList swiftNone = memberAccess.FindByMemberAccess("CxUndefined.None");
swiftNone.Add(memberAccess.FindByMemberAccess("Optional.none", false));
result.Add(swiftNone.FindByFileName("*.swift"));