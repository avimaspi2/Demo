CxList write = 	
	Find_Methods().FindByShortName("Print", false) - All.FindByName("*.Print", false) + 
	All.FindByMemberAccess("object.write*");
result = write;