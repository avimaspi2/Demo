result = Find_Integers();
	//regex to replace
result.Add(All.FindByMemberAccess("Matcher.replaceAll"));
result.Add(All.FindByMemberAccess("String.replaceAll"));
result.Add(All.FindByMemberAccess("Matcher.replaceFirst"));
result.Add(All.FindByMemberAccess("Matcher.appendReplacement"));

//scala.util.matching.Regex replace
CxList patterns = All.FindByMemberAccess("StringLiteral.r").GetAncOfType(typeof(Declarator));
patterns.Add(All.FindByType("Regex").GetAncOfType(typeof(Declarator)));
patterns = All.FindAllReferences(patterns);
CxList patternsMethods = patterns.GetMembersOfTarget();

result.Add(patternsMethods.FindByShortName("replaceAllIn"));
result.Add(patternsMethods.FindByShortName("replaceFirstIn"));
result.Add(patternsMethods.FindByShortName("findAllIn"));
result.Add(patternsMethods.FindByShortName("findFirstIn"));

result.Add(All.FindByMemberAccess("Regex.replaceAllIn"));
result.Add(All.FindByMemberAccess("Regex.replaceFirstIn"));


	//regex filter
result.Add(All.FindByMemberAccess("Matcher.group"));
result.Add(All.FindByMemberAccess("String.split"));
result.Add(All.FindByMemberAccess("StringLiteral.split"));
result.Add(All.FindByMemberAccess("Pattern.split"));