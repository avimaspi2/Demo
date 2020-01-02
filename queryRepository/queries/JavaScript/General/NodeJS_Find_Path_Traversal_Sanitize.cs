CxList mE = Find_Methods();
CxList replace = mE.FindByShortName("replace*", false);
CxList replaceString = NodeJS_Find_Strings();
replaceString = replaceString.FindByShortNames(
	new List<string> {
		"\"/\"",
		"\"\\\""
		}
	);

replace = replace.FindByParameters(replaceString.GetParameters(replace, 0));
CxList replaceTarget = replace.GetTargetOfMembers();

result = NodeJS_Find_General_Sanitize();
result.Add(replaceTarget);