if(AllMembers.All.FindByLanguage("CSharp").Count == 0)
{
	CxList webConfig = Find_Web_Config();
	CxList value_true = webConfig.FindByName("true", false).FindByType(typeof(StringLiteral));
	CxList compilation_Debug = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.COMPILATION.DEBUG", false);

	result = value_true * value_true.DataInfluencingOn(compilation_Debug);
}