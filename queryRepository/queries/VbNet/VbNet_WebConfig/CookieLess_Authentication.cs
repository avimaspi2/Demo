CxList webConfig = Find_Web_Config();
CxList value_Forms = webConfig.FindByName("Forms", false).FindByType(typeof(StringLiteral));
CxList value_UseUri = webConfig.FindByName("UseUri", false).FindByType(typeof(StringLiteral));

CxList formCookieless_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.COOKIELESS", false);
CxList forms_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS", false);
CxList forms_childs = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.*", false);
CxList configuration = webConfig.FindByName("CONFIGURATION", false);

CxList mode_forms = value_Forms.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.MODE", false));
CxList formName = value_UseUri.DataInfluencingOn(formCookieless_exist);

if (formCookieless_exist.Count == 0)
{
	if (forms_exist.Count > 0)
	{
		result = forms_exist - forms_exist.GetByAncs(forms_childs);
	}
}
else
{
	if ((mode_forms + formName).Count >= 2)
	{
		result = value_UseUri * formName;
	}
}