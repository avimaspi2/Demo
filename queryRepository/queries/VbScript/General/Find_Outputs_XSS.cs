CxList methods = All.FindByType(typeof(MethodInvokeExpr));

result = 
	methods.FindByMemberAccess("document.create") + 
	methods.FindByMemberAccess("document.createElement") + 
	All.FindByMemberAccess("document.url") +
	All.FindByMemberAccess("document.urlunencoded") +
	
	methods.FindByMemberAccess("document.write") + 
	methods.FindByMemberAccess("document.writeln") + 
	All.FindByMemberAccess("document.body") + 
	
	All.FindByShortName("innerhtml") + 
	All.FindByShortName("innertext");