CxList methods = Find_Methods();

result = methods.FindByShortName("strncat")
	+ methods.FindByShortName("_strncat*")
	+ methods.FindByShortName("_mbsncat*")
	+ methods.FindByShortName("wcsncat*");