// $ASP*

CxList Inputs = Find_Inputs();

CxList methods = Find_Methods();
CxList sleep = All.GetParameters(methods.FindByShortNames(
	new List<string>{"setTimeout","setInterval"}), 1);

// Adding jQuery.sap.delayedCall and jQuery.sap.intervalCall:
CxList sap = Find_SAP_Library();
CxList members = sap.GetMembersOfTarget();
CxList sapSleepMethods = (methods * members).FindByShortNames(new List<string> { "delayedCall", "intervalCall"});
sleep.Add(All.GetParameters(sapSleepMethods, 0));
result = sleep.DataInfluencedBy(Inputs);