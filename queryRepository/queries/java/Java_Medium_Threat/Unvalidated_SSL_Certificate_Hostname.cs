CxList verifiers = All.InheritsFrom("HostnameVerifier");

CxList inVerifiers = All.GetByAncs(verifiers);
CxList verifyMethods = inVerifiers.FindByType(typeof(MethodDecl)).FindByShortName("verify");
CxList inVerifyMethods = All.GetByAncs(verifyMethods);

CxList stringVerifications = inVerifyMethods.FindByMemberAccess("String.equals");
stringVerifications.Add(inVerifyMethods.FindByMemberAccess("String.matches"));

CxList unsafeVerifiers = verifiers - stringVerifications.GetAncOfType(typeof(ClassDecl));

result = unsafeVerifiers;