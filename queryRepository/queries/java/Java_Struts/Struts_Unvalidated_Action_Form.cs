// Test struts version
if(Find_Struts1_Presence().Count > 0){
	CxList strings = Find_Strings();
	
	CxList formBeanName = All.FindByMemberAccess("FORM_BEAN.NAME").FindByFileName("*struts-config.xml");
	CxList formBeans = strings * strings.DataInfluencingOn(formBeanName);
	CxList validationFormName = All.FindByMemberAccess("FORM.NAME").FindByFileName("*validation.xml");
	CxList validationForms = strings * strings.DataInfluencingOn(validationFormName);
	
	foreach (CxList formBean in formBeans)
	{
		StringLiteral str = formBean.TryGetCSharpGraph<StringLiteral>();
		string strName = str.ShortName.Trim(new char[] {'"'});
		if (validationForms.FindByShortName('"' + strName + '"').Count == 0)
		{
			result.Add(str.NodeId, str);
		}
	}
}