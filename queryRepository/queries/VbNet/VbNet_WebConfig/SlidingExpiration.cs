CxList webConfig = Find_Web_Config();
CxList value_Forms = webConfig.FindByName("Forms", false).FindByType(typeof(StringLiteral));
CxList value_true = webConfig.FindByName("true", false).FindByType(typeof(StringLiteral));

CxList slidingExpiration_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.SLIDINGEXPIRATION", false);
CxList forms_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS", false);
CxList forms_childs = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.*", false);
CxList configuration = webConfig.FindByName("CONFIGURATION", false);

CxList mode_forms = value_Forms.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.MODE", false));
CxList slidingExpiration = value_true.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.SLIDINGEXPIRATION", false));

if (slidingExpiration_exist.Count == 0)
{
	if (forms_exist.Count > 0)
	{
		result = forms_exist - forms_exist.GetByAncs(forms_childs);
	}
}
else
{
	if ((mode_forms + slidingExpiration).Count >= 2)
	{
		result = value_true * slidingExpiration;
	}
}