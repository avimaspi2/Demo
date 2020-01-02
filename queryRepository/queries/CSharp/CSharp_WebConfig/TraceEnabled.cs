CxList webConfig = Find_Web_Config();
CxList value_false = webConfig.FindByName("false").FindByType(typeof(StringLiteral));
CxList value_true = webConfig.FindByName("true").FindByType(typeof(StringLiteral));

CxList enabledTrue = value_true.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.TRACE.ENABLED"));
CxList localOnlyFalse = value_false.DataInfluencingOn(webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.TRACE.LOCALONLY"));

if ((enabledTrue + localOnlyFalse).Count > 1)
{
	result = value_true * enabledTrue;
	}
if ((enabledTrue + localOnlyFalse).Count == 1)
{
	result = (value_true * enabledTrue) + (value_false * localOnlyFalse);
}