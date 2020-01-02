CxList sessionState = All.FindByType("HttpSessionState");
sessionState.Add(All.FindByType("HttpSession"));

CxList toRemove = All.FindByType(typeof(TypeRef));
toRemove.Add(All.FindByType(typeof(Declarator)));
toRemove.Add(All.FindByType(typeof(ParamDecl)));
sessionState -= toRemove;

CxList UrMa = All.FindByType(typeof(UnknownReference));
UrMa.Add(All.FindByType(typeof(MemberAccess)));

sessionState.Add(UrMa.FindByShortName("Session"));
sessionState = sessionState * UrMa;
result.Add(sessionState.GetMembersOfTarget().FindByShortName("Abandon"));