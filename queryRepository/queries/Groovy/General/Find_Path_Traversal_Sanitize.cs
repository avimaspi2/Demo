CxList obj = Find_Object_Create().FindByShortName("File*");

CxList replace = Find_Methods().FindByShortName("replace*", false);
CxList replaceString = Find_Strings();
replaceString = replaceString.FindByShortName("\"/\"") + replaceString.FindByShortName("\"\\\"");
replace = replace.FindByParameters(replaceString.GetParameters(replace, 0));

result = Find_General_Sanitize() - obj + replace;