CxList webConfig = Find_Web_Config();
CxList value_Off = webConfig.FindByName("Off").FindByType(typeof(StringLiteral));
CxList customers_mode = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.CUSTOMERRORS.MODE");
	
result = value_Off * value_Off.DataInfluencingOn(customers_mode);