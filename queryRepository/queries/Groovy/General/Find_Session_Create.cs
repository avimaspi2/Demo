CxList ofHttpSessType = All.FindByType("HttpSession") - All.FindByType(typeof(TypeRef));
result = ofHttpSessType.GetMembersOfTarget().FindByShortName("setAttribute");