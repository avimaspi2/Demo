CxList setMethods = Find_Methods().FindByShortName("set*");

setMethods = setMethods.FindByShortName("setgroups") + 
	setMethods.FindByShortName("sethostid") + 
	setMethods.FindByShortName("sethostname") + 
	setMethods.FindByShortName("setuid");
CxList inputs = Find_Interactive_Inputs();

result = inputs.DataInfluencingOn(setMethods);