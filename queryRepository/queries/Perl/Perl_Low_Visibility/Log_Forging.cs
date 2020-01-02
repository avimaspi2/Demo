CxList Inputs = Find_Interactive_Inputs();
CxList Log = Find_Log_Outputs();

// All replaces that contain \r or \n as first parameter should be removed
CxList replace = Find_Methods().FindByShortName("replace*");
CxList replaceEnter = Find_Strings().GetParameters(replace, 0);
replaceEnter = replaceEnter.FindByShortName(@"*\n*") + replaceEnter.FindByShortName(@"*\r*");
replace = replace.FindByParameters(replaceEnter);

CxList sanitize = Find_Integers() + replace;

result = Log.InfluencedByAndNotSanitized(Inputs, sanitize);