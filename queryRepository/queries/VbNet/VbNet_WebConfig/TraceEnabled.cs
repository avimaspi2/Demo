CxList webConfig = Find_Web_Config();
CxList value_false = webConfig.FindByName("false", false).FindByType(typeof(StringLiteral));
CxList value_true = webConfig.FindByName("true", false).FindByType(typeof(StringLiteral));

CxList enabledTrue = value_true.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.TRACE.ENABLED", false));
CxList localOnlyFalse = value_false.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.TRACE.LOCALONLY", false));

if ((enabledTrue + localOnlyFalse).Count > 1)
{
	result = value_true * enabledTrue;
}
if ((enabledTrue + localOnlyFalse).Count == 1)
{
	result = (value_true * enabledTrue) + (value_false * localOnlyFalse);
}