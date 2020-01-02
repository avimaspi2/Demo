/// <summary>
/// Find debug configuration, which was left in production environment.
/// </summary>
CxList djangoConfig = Find_Django_Config();
CxList configDebug = djangoConfig.FindByShortName("DEBUG", true);
CxList trueValue = djangoConfig.FindByType(typeof(BooleanLiteral)).FindByShortName("true");

result.Add(trueValue.DataInfluencingOn(configDebug).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));