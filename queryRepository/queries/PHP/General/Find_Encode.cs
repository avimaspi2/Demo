CxList methods = Find_Methods();

result = methods.FindByShortName("*encode*", false);

CxList removePart = methods.FindByShortNames(new List<string>{"*encoder*", "*utf7_encode*", "utf8_encode", "bson_encode"} , false);
/*
result =
	methods.FindByShortName("*encode*", false) - 
	methods.FindByShortName("*encoder*", false) -
	methods.FindByShortName("*utf7_encode", false) -
	methods.FindByShortName("utf8_encode", false) -
	methods.FindByShortName("bson_encode", false);
*/
result = result - removePart;