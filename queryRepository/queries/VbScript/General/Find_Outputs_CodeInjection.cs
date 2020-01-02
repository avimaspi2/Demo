CxList methods = All.FindByType(typeof(MethodInvokeExpr));

result = All.FindByMemberAccess("window.location") + 
	methods.FindByMemberAccess("window.execscript") + 
	methods.FindByMemberAccess("window.execcommand") + 
	methods.FindByShortName("eval") +
	methods.FindByShortName("execute") +
	methods.FindByShortName("executeglobal") +

	Find_Member_With_Target("WScript.Shell", "Run") +
	Find_Member_With_Target("WScript.Shell", "Exec") +
	Find_Member_With_Target("WScript.Shell", "AppActivate") + 
	
	//methods which can execute code - exist both in VbScript and JavaScript
	All.GetParameters(methods.FindByShortName("setTimeout",false), 0) +
	All.GetParameters(methods.FindByShortName("setInterval",false), 0);