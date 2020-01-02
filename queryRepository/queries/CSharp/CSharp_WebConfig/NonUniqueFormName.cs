CxList webConfig = Find_Web_Config();
CxList value_Forms = webConfig.FindByName("Forms").FindByType(typeof(StringLiteral));
CxList value_ASPXAUTH = webConfig.FindByName(".ASPXAUTH").FindByType(typeof(StringLiteral));

CxList formName_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.NAME");
CxList forms_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS");
CxList forms_childs = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.*");
CxList configuration = webConfig.FindByName("CONFIGURATION");

CxList mode_forms = value_Forms.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.MODE"));
CxList formName = value_ASPXAUTH.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.NAME"));

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