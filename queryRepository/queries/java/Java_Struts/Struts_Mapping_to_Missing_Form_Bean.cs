// Test struts version
if(Find_Struts1_Presence().Count > 0){
	// Find actions mappings (name of action) that point (by the name) to a non-existant form-bean (name)
	CxList strings = Find_Strings();
	CxList strutsConfig = All.FindByFileName("*struts-config.xml");
	
	CxList formBeanName = strutsConfig.FindByMemberAccess("FORM_BEAN.NAME");
	CxList formBeans = strings * strings.DataInfluencingOn(formBeanName);
	CxList actionName = strutsConfig.FindByMemberAccess("ACTION.NAME");
	CxList actions = strings * strings.DataInfluencingOn(actionName);
	
	foreach (CxList action in actions)
	{
		CxList sameName = formBeans.FindByShortName(action);
		if (sameName.Count == 0 )
		{
			result.data.AddRange(action.data);
		}
	}
	
	result -= result.FindByShortName(@"""""");
}