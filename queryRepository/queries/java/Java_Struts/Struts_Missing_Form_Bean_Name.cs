// Test struts version
if(Find_Struts1_Presence().Count > 0){
	// Find action with an empty name
	CxList strings = Find_Strings();
	CxList strutsConfig = All.FindByFileName("*struts-config.xml");
	
	CxList formBeanName = strutsConfig.FindByMemberAccess("FORM_BEAN.NAME");
	CxList formBeans = strings * strings.DataInfluencingOn(formBeanName);
	
	result = formBeans.FindByShortName(@"""""");
}