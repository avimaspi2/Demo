CxList methods = Find_Methods();
methods -= Find_DB();
methods -= Find_Sanitize();
CxList filtered =
	methods.FindByShortName("Add*") +
	methods.FindByShortName("append") +
	methods.FindByShortName("close") +
	methods.FindByShortName("Equals") +
	methods.FindByShortName("getClass") +
	methods.FindByShortName("getName") +
	methods.FindByShortName("GetString") +
	methods.FindByShortName("GetType") +
	methods.FindByShortName("IndexOf") +
	methods.FindByShortName("LastIndexOf") +
	methods.FindByShortName("length") +
	methods.FindByShortName("prepare*") +
	methods.FindByShortName("read") +
	methods.FindByShortName("replace*") +
	methods.FindByShortName("Set*") +
	methods.FindByShortName("size") +
	methods.FindByShortName("substr") +
	methods.FindByShortName("ToArray") +
	methods.FindByShortName("ToInt") +
	methods.FindByShortName("tolower") +
	methods.FindByShortName("toString") +
	methods.FindByShortName("toupper");

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

result.Add(Find_DB_PostgreSQL_libpq());
result.Add(Find_DB_PostgreSQL_libpqxx());