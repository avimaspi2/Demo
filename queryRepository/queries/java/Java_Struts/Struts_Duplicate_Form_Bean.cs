// Test struts version
if(Find_Struts1_Presence().Count > 0){
	CxList strings = Find_Strings();
	
	CxList formBeanName = All.FindByMemberAccess("FORM_BEAN.NAME").FindByFileName("*struts-config.xml");
	CxList formBeans = strings * strings.DataInfluencingOn(formBeanName);
	
	SortedList beanNamesList = new SortedList();
	foreach (CxList formBean in formBeans)
	{
		StringLiteral str = formBean.TryGetCSharpGraph<StringLiteral>();
		string strName = str.ShortName;
		if (beanNamesList.ContainsKey(strName))
		{
			result.Add(formBean.Concatenate(beanNamesList[strName] as CxList));
		}
		else
		{
			beanNamesList[strName] = formBean;
		}
	}
}