CxList encode = All.FindByName("*.UrlEncode") + 
	All.FindByMemberAccess("URLEncoder.encode") +
	All.FindByShortName("*UrlEncode*", false) +
	All.FindByMemberAccess("Encoder.encodeForURL");

encode = encode.FindByType(typeof(MemberAccess)) + encode.FindByType(typeof(MethodInvokeExpr));

result = encode;