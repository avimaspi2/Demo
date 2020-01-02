CxList template = All.FindByShortName("*template*", false);
CxList invokes = Find_Methods();
CxList templateInvoke = template * invokes;
CxList stringL = Find_String_Literal();
CxList ByIdOrClassName = stringL.FindByShortNames(new List<string>{"#*",".*"});
CxList relevantFlow = templateInvoke.DataInfluencedBy(ByIdOrClassName);
result = relevantFlow;