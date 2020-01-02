CxList encode = All.FindByName("*.UrlEncode");
encode.Add(All.FindByMemberAccess("URLEncoder.encode"));
encode.Add(All.FindByShortName("*UrlEncode*", false));
encode.Add(All.FindByMemberAccess("Encoder.encodeForURL"));

result = encode.FindByType(typeof(MemberAccess));
result.Add(encode.FindByType(typeof(MethodInvokeExpr)));