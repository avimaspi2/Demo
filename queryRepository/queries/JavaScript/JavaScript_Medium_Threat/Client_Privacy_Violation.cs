// This query searches for variables and constants that could contain personal sensitive data which is streamed to an output.

CxList strings = Find_String_Literal();
CxList integerLiteral = Find_IntegerLiterals();
CxList nullLiteral = Find_NullLiteral();
CxList literals = All.NewCxList();
literals.Add(strings);
literals.Add(integerLiteral);
literals.Add(nullLiteral);

// Find names that are suspected to be personal info, eg. String PASSWORD, Integer SSN, and remove string literals, such as x="password"
CxList personal_info = Find_Personal_Info() - strings;
personal_info.Add(personal_info.GetRightmostMember());

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
// * Remark: in all VB languages (including ASP), Ruby, PL/SQL, Typescript and Perl, constants MUST be assigned in their declaration line
CxList constants = personal_info.FindByType(typeof(ConstantDecl));
CxList allConstRef = personal_info.FindAllReferences(constants);
CxList allConstRefOrigin = allConstRef.Clone();
// Find all assignments of null, string or integer literals
CxList ConstAssignLiteral = literals.FindByFathers(allConstRef.FindByType(typeof(Declarator)));

// remove assignments of constants to null, string or integer literals
allConstRef -= personal_info.FindAllReferences(ConstAssignLiteral.GetFathers());

// find all assignments (AssignExpr) to constants
CxList constAssignments = allConstRef.FindByAssignmentSide(CxList.AssignmentSide.Left).GetFathers();

// find assignments of literals to constant personal_info and remove them from results
CxList PI_literal = literals.GetFathers() * constAssignments;
allConstRef -= allConstRef.FindAllReferences(allConstRef.FindByFathers(PI_literal));
// remove from personal_info all references that were removed above
personal_info -= (allConstRefOrigin - allConstRef);

CxList inputs = Find_Inputs();

CxList inputsOutputs = All.NewCxList();
inputsOutputs.Add(inputs);
inputsOutputs.Add(Find_DB_Out());
inputsOutputs.Add(Find_Potential_Inputs());

personal_info = personal_info.DataInfluencedBy(inputsOutputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly) + 
	personal_info * inputsOutputs;

// 3) Add exceptions (that could be thrown) to outputs. 
CxList objectCreate = Find_ObjectCreations();
CxList exceptions = objectCreate.FindByName("*Exception");
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

CxList methods = Find_Methods();
CxList consoleLogs = methods.FindByMemberAccess("console.log");

// Define outputs
CxList outputs = Find_Outputs();
outputs.Add(exceptions);
outputs.Add(exceptionsCtorsWithSuper);
outputs.Add(consoleLogs);

// Define sanitize
CxList sanitize = Find_DB(); // in some languages is called Find_DB, Find_DB_In, Find_DB_Input
CxList encrypt = methods.FindByShortName("*crypt*", false); // crypt is a PHP function used to encrypt strings, and all variables labelled crypt(ed) are considered safe, as well as DBMS_CRYPTO as output
encrypt.Add(methods.FindByShortName("*CipherOutputStream*", false));	// CipherOutputStreamis a Java class used to encrypt output streams
encrypt.Add(All.FindByMemberAccess("MessageDigest.digest", false));	// Java.Security.MessageDigest is a Java class used to encrypt data
encrypt -= encrypt.FindByShortName("*decrypt*");
CxList encoded = methods.FindByShortName("*Encode*", false); // all methods labelled encode(ed) are considered safe
CxList encRemove = encoded.FindByShortName("*UnEncode*", false); 
encRemove.Add(encoded.FindByShortName("*Decode*", false)); 
encRemove.Add(encoded.FindByShortName("*URLEncode*", false)); // URLEncode method is a part of .NET and is not a sanitizer
encRemove.Add(encoded.FindByShortName("*HTMLEncode*", false)); // HTMLEncode method is a part of .NET and is not a sanitizer
encRemove.Add(encoded.FindByShortName("*EncodeHTML*", false)); 
encoded -= encRemove;
sanitize.Add(encrypt);
sanitize.Add(encoded);

// split personal_info into variables and constants
CxList variableRef = personal_info - allConstRef;
variableRef.Add(Find_Passwords());

// find declarators of constants and variables so they can be removed - declarators are not a part of the flow from input to output
// eg. string x = ___ is parsed as: (Declarator) string (UnknownReference) x (AssignExpr) = (value / expression / literal)___
// the real flow is from the UnknownReference and not the Declarator
CxList declarator = personal_info.FindByType(typeof(Declarator));

// remove the declaration from the references of the variables and constants
variableRef -= declarator;
allConstRef -= declarator;

// find all constants that are assigned from an input (directly or indirectly) and are influencing an output
CxList ConstInfuelncedByInput = outputs.InfluencedByAndNotSanitized(allConstRef, sanitize).InfluencedByAndNotSanitized(inputs, sanitize);

// find all variables that are influencing an output
CxList variableRefPath = outputs.InfluencedByAndNotSanitized(variableRef, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);

result = variableRefPath;
result.Add(ConstInfuelncedByInput);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);