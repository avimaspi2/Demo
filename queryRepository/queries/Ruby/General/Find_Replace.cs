CxList replace = Find_Methods().FindByShortName("str_replace");

CxList parameters = All.GetParameters(replace, 0).FindByType(typeof(StringLiteral));

parameters = parameters.FindByShortName("'") + parameters.FindByShortName("\\'") + parameters.FindByShortName("\\\\'") +
	parameters.FindByShortName("/'") + parameters.FindByShortName("//'");

result = replace.FindByParameters(parameters);