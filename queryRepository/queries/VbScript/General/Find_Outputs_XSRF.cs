CxList methods = All.FindByType(typeof(MethodInvokeExpr));

result = 
	All.FindByShortName("action") + 
	All.FindByShortName("src") +
	
	All.FindByMemberAccess("document.url") +
	All.FindByMemberAccess("document.urlunencoded") +
	methods.FindByMemberAccess("document.open") +
	All.FindByMemberAccess("location.href") + 
	
	All.FindByMemberAccess("window.location") +
	methods.FindByMemberAccess("window.open") +
	methods.FindByMemberAccess("window.navigate") +
	All.FindByMemberAccess("navigate.href");