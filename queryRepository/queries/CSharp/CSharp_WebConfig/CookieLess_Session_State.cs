CxList webConfig = Find_Web_Config();
CxList value_UseUri = webConfig.FindByName("UseUri").FindByType(typeof(StringLiteral));
CxList sessionState_cookieless = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.SESSIONSTATE.COOKIELESS");

result = value_UseUri * value_UseUri.DataInfluencingOn(sessionState_cookieless);