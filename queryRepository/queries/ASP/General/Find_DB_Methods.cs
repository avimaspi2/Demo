CxList methods = Find_Methods();
methods -= Find_DB();
methods -= Find_Sanitize();
CxList filtered = 
	methods.FindByShortName("add*") +
	methods.FindByShortName("append") +
	methods.FindByShortName("close") +
	methods.FindByShortName("contains") +
	methods.FindByShortName("equals") +
	methods.FindByShortName("exists") +
	methods.FindByShortName("getname") +
	methods.FindByShortName("getstring") +
	methods.FindByShortName("gettype") +
	methods.FindByShortName("indexof") +
	methods.FindByShortName("lastindexof") +
	methods.FindByShortName("length") +
	methods.FindByShortName("prepare*") +
	methods.FindByShortName("preparestatement") +
	methods.FindByShortName("read*") +
	methods.FindByShortName("replace") +
	methods.FindByShortName("set*") +
	methods.FindByShortName("size") +
	methods.FindByShortName("substring") +
	methods.FindByShortName("toarray") +
	methods.FindByShortName("toint") +
	methods.FindByShortName("tolower") +
	methods.FindByShortName("tostring") +
	methods.FindByShortName("toupper") +
	methods.FindByShortName("write") +
	methods.FindByShortName("writeline");

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