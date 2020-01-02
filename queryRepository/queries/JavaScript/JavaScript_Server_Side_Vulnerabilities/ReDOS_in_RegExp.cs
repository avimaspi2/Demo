//RegExp has the following methods: exec, and test and is activated like this: RegExpObject.exec(string)
CxList methods = Find_Methods();
CxList execOrTest = methods.FindByShortNames(new List<string>{"exec","test"});

//now the conditions:
//get the parameter of the exec and test:
CxList inputs = NodeJS_Find_Inputs();
CxList unknownReference = Find_UnknownReference();
CxList sanitize = Sanitize();
//in case they are influenced by inputs
CxList paramInfluencedByInputs = unknownReference
	.GetByAncs(All.GetParameters(execOrTest))
	.InfluencedByAndNotSanitized(inputs, sanitize);

//now get to its regex and see if it's influenced by evil strings
CxList regex = execOrTest.FindByParameters(paramInfluencedByInputs).GetTargetOfMembers();
CxList evil = Find_Evil_Strings();
result.Add(regex * evil);
result.Add(regex.DataInfluencedBy(evil));

//get NodeJS handlers using evil strings
foreach(CxList evilRegex in evil){
	CxList evilVars = evilRegex.GetAncOfType(typeof(Declarator));
	evilVars.Add(Find_Assign_Lefts().GetByAncs(evil.GetAncOfType(typeof(AssignExpr))));
	CxList regexEvil = All.FindAllReferences(evilVars) * regex;
	
	if(regexEvil.Count > 0){
		result.Add(evilRegex.ConcatenatePath(evilVars, false)
			.ConcatenatePath(regexEvil, false)
			.ConcatenatePath(regexEvil.GetMembersOfTarget(), false));
	}
}