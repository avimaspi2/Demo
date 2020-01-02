CxList webConfig = Find_Web_Config();
CxList userName_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.CREDENTIALS.USER.NAME");
CxList password_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.CREDENTIALS.USER.PASSWORD");

if ((userName_exist + password_exist).Count == 1)
{
	result = userName_exist + password_exist;
}
if ((userName_exist + password_exist).Count > 1)
{
	result = userName_exist;
}