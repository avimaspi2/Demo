result = 
	Find_Integers() + 
	All.FindByShortName("checkUrl") +
	All.FindByShortName("testUrl") +
	All.FindByMemberAccess("*Request.getContextPath");