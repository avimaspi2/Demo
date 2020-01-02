// This query searches for variables an constants that could contain personal sensitive data which is streamed to an output.

CxList strings = Find_Strings();
CxList integerLiteral = All.FindByType(typeof(IntegerLiteral));
CxList nullLiteral = All.FindByType(typeof(NullLiteral));
CxList methods = Find_Methods();
CxList literals = All.NewCxList();
literals.Add(strings);
literals.Add(integerLiteral);

// Find names that are suspected to be personal info, eg. String PASSWORD, Integer SSN, and remove string literals, such as x="password"
CxList personal_info = Find_Personal_Info() - strings;

// 1)exclude variables that are all uppercase - usually describes the pattern of the data, such as PASSWORDPATTERN, PASSORDTYPE...
CxList upperCase = All.NewCxList();
string name;
foreach (CxList res in personal_info)
{
	name = res.GetName();
	if (name.ToUpper().Equals(name))
	{
		upperCase.Add(res);
	}
}
personal_info -= upperCase;

// 2) exclude constants that are assigned a literal
CxList constants = personal_info.FindByType(typeof(ConstantDecl));
CxList allConstRef = personal_info.FindAllReferences(constants);
CxList allConstRefOrigin = allConstRef;
// Find all assignments of string or integer literals
CxList intStringLiteral = strings;
intStringLiteral.Add(integerLiteral);
CxList ConstAssignedL = intStringLiteral.FindByFathers(allConstRef.FindByType(typeof(Declarator)));

// remove assignments of constants to string or integer literals
allConstRef -= personal_info.FindAllReferences(ConstAssignedL.GetFathers());

// remove assignments of constants to null literals
CxList declWithNull = allConstRef * nullLiteral.GetFathers().FindByType(typeof(Declarator));
// constants are assigned null value by default if the real assignment is not in the declaration line, and so the implicit assignment to null is irrelevant.
// eg. final x; is parsed as final x=null; although x can be assigned later
CxList allReferences = allConstRef.FindAllReferences(declWithNull);
// FindByAssignmentSide(left) finds the real assignments, eg. x = ...
CxList toRemove = declWithNull - allReferences.FindDefinition(allReferences.FindByAssignmentSide(CxList.AssignmentSide.Left));
allConstRef -= toRemove;

// find all assignments to constants
CxList constAssignments = allConstRef.FindByAssignmentSide(CxList.AssignmentSide.Left).GetFathers();

// find assignments of literals to constant personal_info and remove them from results
CxList PI_literal = literals.GetFathers() * constAssignments;
allConstRef -= allConstRef.FindAllReferences(allConstRef.FindByFathers(PI_literal));
// remove from personal_info all references that were removed above
personal_info -= (allConstRefOrigin - allConstRef);

//reduce personal_info to be only personal info that is influenced by input
CxList fromConfig = All.FindByFileName("*app.config").FindByType(typeof (StringLiteral));
CxList inputs = Find_Inputs() + Find_DB_Out() + fromConfig;
personal_info = personal_info.DataInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly) +
	personal_info * inputs;

// 3) Add exceptions (that could be thrown) to outputs. 
CxList exceptions = All.FindByType(typeof(ObjectCreateExpr)).FindByName("*Exception");
CxList exceptionsCtors = All.FindByType(typeof(ConstructorDecl)).FindByName("*Exception");
// handle the case where the super (base) constructor of the exception is used to create a new throwable exception
CxList exceptionsCtorsWithSuper = All.NewCxList();
foreach(CxList ctor in exceptionsCtors){
	try {
		ConstructorDecl c = ctor.data.GetByIndex(0) as ConstructorDecl;
		if(c.BaseParameters.Count > 0){
			exceptionsCtorsWithSuper.Add(ctor);
		}
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}

// Define outputs
CxList outputs = Find_Outputs();
outputs.Add(exceptions);
outputs.Add(exceptionsCtorsWithSuper);

// Define sanitize
CxList sanitize = Find_DB_In(); // in some languages is called Find_DB, Find_DB_In, Find_DB_Input
CxList encrypt = Find_Encrypt();
CxList unsafeEncrypt = Find_Broken_Or_Risky_Cryptographic_Algorithm();
CxList encoded = Find_Encode();
CxList encRemove = encoded.FindByShortName("*URLEncode*", false);// URLEncode method is a part of .NET and is not a sanitizer
encRemove.Add(encoded.FindByShortName("*HTMLEncode*", false)); // HTMLEncode method is a part of .NET and is not a sanitizer
encRemove.Add(encoded.FindByShortName("*EncodeHTML*", false)); // EncodeHTML method is not a sanitizer
encoded -= encRemove;
sanitize.Add(encrypt- unsafeEncrypt);
sanitize.Add(encoded);
// Add methods that return information about the object - and not the object itself
sanitize.Add(methods.FindByShortNames(new List<string> {"nameof", "typeof"}));
sanitize.Add(All.FindByMemberAccess("*.GetType"));
sanitize.Add(All.FindByMemberAccess("*.GetHashCode"));
sanitize.Add(All.FindByMemberAccess("*.Equals"));

// split personal_info into variables and constants
CxList variableRef = personal_info - allConstRef;

// find declarators of constants and variables so they can be removed - declarators are not a part of the flow from input to output
// eg. string x = ___ is parsed as: (Declarator) string (UnknownReference) x (AssignExpr) = (value / expression / literal)___
// the real flow is from the UnknownReference and not the Declarator
CxList declarator = personal_info.FindByType(typeof(Declarator));

// remove the declaration from the references of the variables and constants
variableRef -= declarator;
allConstRef -= declarator;

// find all constants that are assigned from an input (directly or indirectly) and are influencing an output
CxList ConstInfuelncedByInput = outputs.InfluencedByAndNotSanitized(allConstRef, sanitize).InfluencedByAndNotSanitized(Find_Inputs(), sanitize);

// find all variables that are influencing an output
CxList variableRefPath = outputs.InfluencedByAndNotSanitized(variableRef, sanitize);

result = variableRefPath;
result.Add(ConstInfuelncedByInput);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);