// $ASP*

CxList Inputs = Find_Interactive_Inputs();
CxList sleep = All.FindByName("WScript.Sleep", false) + 					// vbs
	All.GetParameters(Find_Methods().FindByShortName("setTimeout"), 1) + 	// js
	All.GetParameters(Find_Methods().FindByShortName("setInterval"), 1); 	// js

result = sleep.DataInfluencedBy(Inputs);