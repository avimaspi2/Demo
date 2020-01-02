CxList methods = All.FindByType(typeof(MethodInvokeExpr));

result = 
	methods.FindByMemberAccess("window.navigate") + 
	All.FindByMemberAccess("navigate.href") + 
	methods.FindByMemberAccess("window.open") + 
	
	methods.FindByMemberAccess("document.open") + 
	All.FindByMemberAccess("document.url") +
	All.FindByMemberAccess("document.urlunencoded") +
	All.FindByShortName("location.href");