CxList methods = Find_Methods();
methods -= Find_DB_Base();
methods -= Find_Sanitize();

CxList filtered = methods.FindByShortNames(new List<string> {
		"add*",
		"append",
		"close",
		"contains",
		"equals",
		"exists",
		"getname",
		"getstring",
		"gettype",
		"indexof",
		"lastindexof",
		"length",
		"prepare*",
		"preparestatement",
		"read*",
		"replace",
		"set*",
		"size",
		"substring",
		"toarray",
		"toint",
		"tolower",
		"tostring",
		"toupper",
		"write",
		"writeline"}, false);

methods -= filtered;

foreach(CxList method in methods)
{	
	CxList targetOfMembers = method.GetTargetOfMembers();
	if((targetOfMembers != null) &&
		(targetOfMembers.Count > 0) &&
		(method.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)).Count == 0))
	{
		result.Add(method);
	}
}