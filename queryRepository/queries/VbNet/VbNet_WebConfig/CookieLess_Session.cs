CxList webConfig = Find_Web_Config();
CxList value_UseUri = webConfig.FindByName("UseUri", false).FindByType(typeof(StringLiteral));
CxList sessionState_cookieless = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.SESSIONSTATE.COOKIELESS", false);

result = value_UseUri * value_UseUri.DataInfluencingOn(sessionState_cookieless);