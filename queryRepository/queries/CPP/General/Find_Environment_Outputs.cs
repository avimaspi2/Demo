// Find environment outputs
CxList methods = Find_Methods();
result = 
	methods.FindByShortName("setenv") + 
	methods.FindByShortName("_putenv_s") +
	methods.FindByShortName("_wputenv_s") 
	;