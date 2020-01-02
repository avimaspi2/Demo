CxList webConfig = Find_Web_Config();
CxList formCookieless_exist = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.AUTHENTICATION.FORMS.COOKIELESS");
result = formCookieless_exist.GetAssigner().FindByShortName("UseUri", false);