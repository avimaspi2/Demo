CxList inputs = Find_GWT_Inputs();
//CxList location = All.FindByMemberAccess("Location.get*");
CxList XSS_GWT_Output = GWT_XSS_Outputs();
CxList sanitize = All.FindByName("*encode*", false);

result = inputs.InfluencingOnAndNotSanitized(XSS_GWT_Output, sanitize);