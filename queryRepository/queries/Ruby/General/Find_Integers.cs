CxList allMethods = Find_Methods() + All.FindByType(typeof(MemberAccess));
CxList numberSanitizer = 
	allMethods.FindByShortName("round", false) + 
	allMethods.FindByShortName("average", false) + 
	allMethods.FindByShortName("maximum", false) + 
	allMethods.FindByShortName("minimum", false) + 
	allMethods.FindByShortName("sum", false) + 
	allMethods.FindByShortName("doubleval", false) +
	allMethods.FindByShortName("strlen", false) + 
	allMethods.FindByShortName("intval", false) + 
	allMethods.FindByShortName("*count*", false) + 
	allMethods.FindByShortName("*size*", false) + 
	allMethods.FindByShortName("*length*", false) + 
	allMethods.FindByShortName("*position*", false);

result = numberSanitizer + Find_Interactive_Inputs().FindByRegex("to_i");;