CxList methods = All.FindByType(typeof(MethodInvokeExpr));

result = 
	All.FindByMemberAccess("window.location") +
	All.FindByMemberAccess("window.navigate") +
	All.FindByMemberAccess("navigate.href") +
	methods.FindByMemberAccess("window.open") +

	All.FindByMemberAccess("location.href") +

	All.FindByMemberAccess("document.location") +
	methods.FindByMemberAccess("document.open") +
	All.FindByMemberAccess("document.referrer") +
	All.FindByMemberAccess("document.url") +
	All.FindByMemberAccess("document.urlunencoded");

result.Add(
	All.FindByName("*request.Form",false) +
	methods.FindByShortName("inputbox",false) +
	All.FindByMemberAccess("stdin.read*") +
	All.FindByName("userinput",false) +
	All.FindByName("getuserinput",false));