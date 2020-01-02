// Test struts version
if(Find_Struts1_Presence().Count > 0){
	// Find relative paths in the config file.
	
	CxList strings = Find_Strings();
	
	CxList path = All.FindByFileName("*struts-config.xml").FindByMemberAccess("*.PATH");
	CxList relevantPathnames = strings * strings.DataInfluencingOn(path);
	
	relevantPathnames -= relevantPathnames.FindByShortName("\"/*");
	relevantPathnames -= relevantPathnames.FindByShortName("\"\\*");
	
	result = relevantPathnames;
}