// This query searches for variables an constants that could contain personal sensitive data which is streamed to an output.

CxList inputs = Find_Inputs();
inputs.Add(Find_DB_Out());

CxList strings = Find_Strings();
CxList integerLiteral = Find_IntegerLiterals();
CxList nullLiteral = Find_NullLiteral();
CxList literals = All.NewCxList();
literals.Add(strings);
literals.Add(integerLiteral);
literals.Add(nullLiteral); // nullLiterals are treated separately

// Find names that are suspected to be personal info, eg. String PASSWORD, Integer SSN, and remove string literals, such as x="password"
CxList personal_info = Find_Personal_Info() - strings;

//limit to personal info influenced by input
personal_info = personal_info.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly) + 
	personal_info * inputs;
// 1)exclude variables that are all uppercase - usually describes the pattern of the data, such as PASSWORDPATTERN, PASSORDTYPE...
CxList upperCase = All.NewCxList();
foreach (CxList res in personal_info)
{
	string name = res.GetName();
	if (name.ToUpper().Equals(name))
	{
		upperCase.Add(res);
	}
}
personal_info -= upperCase;

// 2) exclude constants that are assigned a literal
// Python does not support read-only variables, and so has no constants

// 3) Add exceptions (that could be thrown) to outputs. 
CxList exceptions = Find_ObjectCreations().FindByName("*Exception");
CxList exceptionsCtors = Find_ConstructorDecl().FindByName("*Exception");
// handle the case where the super (base) constructor of the exception is used to create a new throwable exception
CxList exceptionsCtorsWithSuper = All.NewCxList();
foreach(CxList ctor in exceptionsCtors){
	try {
		ConstructorDecl c = ctor.TryGetCSharpGraph<ConstructorDecl>();
		if(c.BaseParameters.Count > 0){
			exceptionsCtorsWithSuper.Add(ctor);
		}
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(" null exception constructor ");
	}
	
}

// Define outputs
CxList outputs = Find_Outputs();
outputs.Add(exceptions);
outputs.Add(exceptionsCtorsWithSuper);

// Define sanitize
CxList sanitize = Find_DB(); // in some languages is called Find_DB, Find_DB_In, Find_DB_Input
sanitize.Add(Find_DB_Conn_Strings());
CxList encrypt = All.FindByShortName("*crypt*", false); // crypt is a PHP function used to encrypt strings, and all variables labelled crypt(ed) are considered safe, as well as DBMS_CRYPTO as output
encrypt.Add(Find_Encrypt());
CxList encRemove = encrypt.FindByShortName("*decrypt*");
encRemove.Add(encrypt.FindByShortName("*unencrypt*"));
encrypt -= encRemove;
CxList encoded = All.FindByShortName("*Encode*", false); // all variables labelled encode(ed) are considered safe
encRemove = encoded.FindByShortName("*UnEncode*", false);;
encRemove.Add(encoded.FindByShortName("*Decode*", false)); 
encRemove.Add(encoded.FindByShortName("*URLEncode*", false)); // URLEncode method is a part of .NET and is not a sanitizer
encRemove.Add(encoded.FindByShortName("*HTMLEncode*", false)); // HTMLEncode method is a part of .NET and is not a sanitizer
encRemove.Add(encoded.FindByShortName("*EncodeHTML*", false)); 
encoded -= encRemove;
sanitize.Add(encrypt);
sanitize.Add(encoded);

// find all Personal Info that are influencing an output
result = outputs.InfluencedByAndNotSanitized(personal_info, sanitize);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);