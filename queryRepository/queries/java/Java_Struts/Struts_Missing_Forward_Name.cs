// Test struts version
if(Find_Struts1_Presence().Count > 0){
	// Find forward with no name, probably leftover debug code
	CxList strings = Find_Strings();
	CxList strutsConfig = All.FindByFileName("*struts-config.xml");
	
	CxList forwardName = strutsConfig.FindByMemberAccess("FORWARD.NAME");
	CxList forward = strings * strings.DataInfluencingOn(forwardName);
	
	result = forward.FindByShortName(@"""""");
}