//Get all method calls (extract, parse_str, mb_parse_str) that can overwrite global variables
CxList possible_overwrite = Find_Methods().FindByShortName("extract");
CxList uref = All.FindByType(typeof(UnknownReference));

//vulnerability exists in PHP extract() methods with flags EXTR_OVERWRITE (default flag) and EXTR_IF_EXISTS.
CxList non_overwrite_flags = uref.FindByShortNames(new List<String>()
	{ "EXTR_SKIP", "EXTR_PREFIX_SAME", "EXTR_PREFIX_ALL", "EXTR_PREFIX_INVALID", "EXTR_PREFIX_IF_EXISTS", "EXTR_REFS" });
CxList toRemove = non_overwrite_flags.GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("extract");

//vulnerability exists in methods parse_str and mb_parse_str when called without result parameter.
//When there is no result parameter, values are set on global variables.
CxList allParams = Find_Params();
CxList parseMethod = Find_Methods().FindByShortNames(new List<string>{"parse_str","mb_parse_str"});
foreach (CxList method in parseMethod) {
	if (allParams.GetParameters(method).Count < 2) {
		possible_overwrite.Add(method);
	}
}

toRemove.Add(possible_overwrite.GetTargetOfMembers().GetMembersOfTarget());
toRemove.Add(possible_overwrite.GetMembersOfTarget().GetTargetOfMembers());
possible_overwrite -= toRemove;

// Get all methods calls that have an input in the parameter and are already in query Improper_Control_of_Dynamically_Identified_Variables
CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_General_Sanitize();
CxList possible_overwrite_inputs = possible_overwrite.InfluencedByAndNotSanitized(inputs, sanitize);

result = possible_overwrite - possible_overwrite_inputs;