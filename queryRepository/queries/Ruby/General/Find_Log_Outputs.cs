CxList log = 
	All.FindByMemberAccess("Logger.debug") + 
	All.FindByMemberAccess("Logger.error");

result = All.GetParameters(log);