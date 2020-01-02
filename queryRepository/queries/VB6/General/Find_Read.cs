CxList read = 	
	Find_Methods().FindByShortName("Input", false) - All.FindByName("*.Input", false) + 
	All.FindByMemberAccess("object.read*");

result = read;