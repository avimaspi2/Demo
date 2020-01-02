if(AllMembers.All.FindByLanguage("CSharp").Count == 0)
{
	CxList webConfig = Find_Web_Config();
	CxList value_Off = webConfig.FindByName("Off", false).FindByType(typeof(StringLiteral));
	CxList customers_mode = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.CUSTOMERRORS.MODE", false);
	
	result = value_Off * value_Off.DataInfluencingOn(customers_mode);
}