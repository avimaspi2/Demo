CxList inp = Find_Interactive_Inputs();
CxList file = 
	Find_Methods().FindByShortName("Open", false) - All.FindByName("*.Open", false) + 
	All.FindByShortName("*file*", false);

result = All.FindByShortName("*file*", false).DataInfluencedBy(inp);