CxList methods = Find_Methods();
methods -= Find_DB_In();
methods -= Find_Sanitize();

CxList filtered = methods.FindByShortNames(new List<string> {
		"add*",
		"append",
		"before*",
		"charAt",
		"close",
		"clearParameters",
		"Contains",
		"createQuery",
		"equals",
		"exists",
		"first",
		"getClass",
		"getName",
		"getRow",
		"getString",
		"getType",
		"has*",
		"indexOf",
		"iterator",
		"last*",
		"length",
		"make*",
		"next*",
		"prepare*",
		"print",
		"println",
		"read",
		"readLine",
		"replace*",
		"set*",
		"size",
		"startsWith",
		"substring",
		"toArray",
		"toInt*",
		"toLower*",
		"toString",
		"toUpperCase",
		"validate",
		"valueOf",
		"write",
		"reportError"});
			
filtered.Add(methods.FindByShortNames(new List<string>{"printstack*", "info*", "print*"}, false));
filtered.Add(methods.GetMembersWithTargets(All.FindByType("StringBuilder"))); //remove methods of the class StringBuilder

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