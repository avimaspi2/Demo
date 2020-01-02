CxList inputs = Find_Interactive_Inputs();
CxList extract = Find_Methods().FindByShortName("extract");
CxList uref = All.FindByType(typeof(UnknownReference));

CxList non_overwrite_flags = uref.FindByShortNames(new List<String>()
	{ "EXTR_SKIP", "EXTR_PREFIX_SAME", "EXTR_PREFIX_ALL", "EXTR_PREFIX_INVALID", "EXTR_PREFIX_IF_EXISTS", "EXTR_REFS" });

CxList extractNonOverwriteFlags = non_overwrite_flags.GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("extract");
	
//vulnerability exists only in PHP extract() methods with flags EXTR_OVERWRITE (default flag) and EXTR_IF_EXISTS.
extract -= extractNonOverwriteFlags;
extract -= extract.GetTargetOfMembers().GetMembersOfTarget();
extract -= extract.GetMembersOfTarget().GetTargetOfMembers();

CxList sanitize = Find_General_Sanitize();
result = extract.InfluencedByAndNotSanitized(inputs, sanitize);