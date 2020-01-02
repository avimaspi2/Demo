CxList webConfig = Find_Web_Config();
CxList value_Forms = webConfig.FindByName("Forms").FindByType(typeof(StringLiteral));
CxList value_true = webConfig.FindByName("true").FindByType(typeof(StringLiteral));

CxList slidingExpiration_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.SLIDINGEXPIRATION");
CxList forms_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS");
CxList forms_childs = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.*");
CxList configuration = webConfig.FindByName("CONFIGURATION");

CxList mode_forms = value_Forms.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.MODE"));
CxList slidingExpiration = value_true.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.SLIDINGEXPIRATION"));

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