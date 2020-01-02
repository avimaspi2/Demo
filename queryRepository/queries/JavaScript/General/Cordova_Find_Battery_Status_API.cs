/// <summary>
/// This query finds PhoneGap's Battery Status API usages.
/// Reference: https://cordova.apache.org/docs/en/latest/reference/cordova-plugin-battery-status/index.html
/// </summary>

/* Find methods */
CxList methods = Find_Methods();

/* Get all battery status event listeners */
CxList listener = Find_UnknownReference().FindByShortName("window")
	.GetMembersOfTarget().FindByShortName("addEventListener");
List<string> batteryEvent = new List<string> {"batterystatus", "batterylow" , "batterycritical"};

/* Get all battery status event */
CxList batteryEventParams = Find_String_Literal().GetParameters(listener, 0).FindByShortNames(batteryEvent);

/* Get all battery status event first parameter - status object */
CxList batteryHandlingDefinition = All.FindDefinition(All.GetParameters(methods.FindByParameters(batteryEventParams), 1));

/* Get all the status object usages (level and isPlugged) */
result = All.GetParameters(batteryHandlingDefinition, 0);
result.Add(All.GetParameters(All.FindDefinition(All.FindAllReferences(batteryHandlingDefinition).GetAssigner(Find_LambdaExpr())), 0));