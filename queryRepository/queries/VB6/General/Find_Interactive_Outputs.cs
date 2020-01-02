result = 
	Find_Methods().FindByShortName("MsgBox",false) + 
	All.FindByName("*.text") + 
	Find_Console_Outputs() + Find_Web_Outputs();