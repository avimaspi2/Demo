CxList cattr = All.FindByType(typeof(CustomAttribute));
CxList cathrows = cattr.FindByShortName("CxThrows");
result = cathrows.GetAncOfType(typeof(MethodDecl));