// Test struts version
if(Find_Struts1_Presence().Count > 0){
	// Find action with an empty type
	CxList strings = Find_Strings();
	CxList strutsConfig = All.FindByFileName("*struts-config.xml");
	
	CxList formBeanType = strutsConfig.FindByMemberAccess("FORM_BEAN.TYPE");
	CxList formBeans = strings * strings.DataInfluencingOn(formBeanType);
	
	result = formBeans.FindByShortName(@"""""");
}