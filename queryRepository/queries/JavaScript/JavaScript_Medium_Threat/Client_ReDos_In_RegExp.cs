//RegExp has the following methods: exec, and test and is activated like this: RegExpObject.exec(string)
CxList methods = Find_Methods();
CxList execOrTest = methods.FindByShortNames(new List<string>{"exec","test"});

//now the conditions:
//get the parameter of the exec and test:
CxList inputs = Find_Inputs();
CxList unknownReference = Find_UnknownReference();
CxList sanitize = Sanitize();
//in case they are influenced by inputs
CxList paramInfluencedByInputs = unknownReference.GetByAncs(All.GetParameters(execOrTest)).InfluencedByAndNotSanitized(inputs, sanitize);

//now get to its regex and see if it's influenced by evil strings
CxList regex = execOrTest.FindByParameters(paramInfluencedByInputs).GetTargetOfMembers();
CxList evil = Find_Evil_Strings();
result.Add(regex * evil);
result.Add(regex.DataInfluencedBy(evil));