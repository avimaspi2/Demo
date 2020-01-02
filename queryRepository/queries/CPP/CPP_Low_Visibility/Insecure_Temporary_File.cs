CxList methods = Find_Methods();

result = methods.FindByShortName("_mktemp")
	+ methods.FindByShortName("_tempnam")
	+ methods.FindByShortName("_tmpfile")
	+ methods.FindByShortName("_tmpnam")
	+ methods.FindByShortName("_ttempnam")
	+ methods.FindByShortName("_ttmpnam")
	+ methods.FindByShortName("_wtempnam")
	+ methods.FindByShortName("_wtmpnam")
	+ methods.FindByShortName("GetTempFileName")
	+ methods.FindByShortName("mkstemp")
	+ methods.FindByShortName("mktemp")
	+ methods.FindByShortName("tempnam")
	+ methods.FindByShortName("tmpfile")
	+ methods.FindByShortName("tmpnam")
	+ methods.FindByShortName("tmpnam_r");