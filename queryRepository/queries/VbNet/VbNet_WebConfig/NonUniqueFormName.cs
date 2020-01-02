CxList webConfig = Find_Web_Config();
CxList value_Forms = webConfig.FindByName("Forms", false).FindByType(typeof(StringLiteral));
CxList value_ASPXAUTH = webConfig.FindByName(".ASPXAUTH", false).FindByType(typeof(StringLiteral));

CxList formName_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.NAME", false);
CxList forms_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS", false);
CxList forms_childs = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.*", false);
CxList configuration = webConfig.FindByName("CONFIGURATION", false);

CxList mode_forms = value_Forms.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.MODE", false));
CxList formName = value_ASPXAUTH.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.NAME", false));

if (formName_exist.Count == 0)
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
		result = value_ASPXAUTH * formName;
	}
}