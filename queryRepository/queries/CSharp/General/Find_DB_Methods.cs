CxList methods = Find_Methods();
methods -= Find_DB_Base();
methods -= Find_SQL_Sanitize();

List<string> names = new List<string> {
		"Add*",
		"Append",
		"Close",
		"Contains",
		"Dispose",
		"Equals",
		"Exists",
		"GetName",
		"GetString",
		"GetType",
		"IndexOf",
		"LastIndexOf",
		"Length",
		"prepare*",
		"PrepareStatement",
		"Read*",
		"Replace",
		"Set*",
		"Size",
		"Substring",
		"ToArray",
		"ToInt",
		"ToLower",
		"ToString",
		"ToUpper",
		"Write",
		"WriteLine",
		"Init",
		"Load",
		"PreRender",
		"Render"
		};

methods -= methods.FindByShortNames(names);
methods -= methods.FindByMemberAccess("MessageBox.Show");

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