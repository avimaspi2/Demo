CxList methods = Find_Methods();
methods -= Find_DB_In();
methods -= Find_Sanitize();
CxList filtered = methods.FindByShortName("add*") +
	methods.FindByShortName("append") +
	methods.FindByShortName("before*") +
	methods.FindByShortName("charAt") +
	methods.FindByShortName("close") +
	methods.FindByShortName("clearParameters") +
	methods.FindByShortName("Contains") +
	methods.FindByShortName("createQuery") +
	methods.FindByShortName("equals") +
	methods.FindByShortName("exists") +
	methods.FindByShortName("first") +
	methods.FindByShortName("getClass") +
	methods.FindByShortName("getName") +
	methods.FindByShortName("getRow") +
	methods.FindByShortName("getString") +
	methods.FindByShortName("getType") +
	methods.FindByShortName("has*") +
	methods.FindByShortName("indexOf") +
	methods.FindByShortName("iterator") +
	methods.FindByShortName("last*") +
	methods.FindByShortName("length") +
	methods.FindByShortName("make*") +
	methods.FindByShortName("next*") +
	methods.FindByShortName("prepare*") +
	methods.FindByShortName("print") +
	methods.FindByShortName("println") +
	methods.FindByShortName("read") +
	methods.FindByShortName("readLine") +
	methods.FindByShortName("replace*") +
	methods.FindByShortName("set*") +
	methods.FindByShortName("size") +
	methods.FindByShortName("startsWith") +
	methods.FindByShortName("substring") +
	methods.FindByShortName("toArray") +
	methods.FindByShortName("toInt*") +
	methods.FindByShortName("toLower*") +
	methods.FindByShortName("toString") +
	methods.FindByShortName("toUpperCase") +
	methods.FindByShortName("validate") +
	methods.FindByShortName("valueOf") +
	methods.FindByShortName("write");

methods -= filtered;

methods -= methods.FindAllReferences(All.FindDefinition(methods)); // Remove methods that have an implementation

foreach(CxList method in methods)
{	
	CxList targetOfMembers = method.GetTargetOfMembers();
	if((targetOfMembers != null) &&
		(targetOfMembers.Count > 0) &&
		(method.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)).Count == 0))
	{
		result.data.AddRange(method.data);
	}
}