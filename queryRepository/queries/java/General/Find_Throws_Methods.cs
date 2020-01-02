CxList cattr = Find_CustomAttribute();
CxList cathrows = cattr.FindByShortName("CxThrows");
result = cathrows.GetAncOfType(typeof(MethodDecl));