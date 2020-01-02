CxList webConfig = Find_Web_Config();
CxList value_Forms = webConfig.FindByName("Forms").FindByType(typeof(StringLiteral));
CxList value_false = webConfig.FindByName("false").FindByType(typeof(StringLiteral));

CxList requireSSL_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.REQUIRESSL");
CxList forms_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS");
CxList forms_childs = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.*");
CxList configuration = webConfig.FindByName("CONFIGURATION");

CxList mode_forms = value_Forms.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.MODE"));
CxList requireSSL = value_false.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.REQUIRESSL"));

if (requireSSL_exist.Count == 0)
{
	if (forms_exist.Count > 0)
	{
		result = forms_exist - forms_exist.GetByAncs(forms_childs);
	}
}
else
{
	if ((mode_forms + requireSSL).Count >= 2)
	{
		result = value_false * requireSSL;
	}
}