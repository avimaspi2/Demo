/*checks whether verification of the SSL is set to verify_none*/
CxList verify = All.FindByShortName("verify_none", false);
result = verify.GetTargetOfMembers().FindByShortName("*ssl", false);