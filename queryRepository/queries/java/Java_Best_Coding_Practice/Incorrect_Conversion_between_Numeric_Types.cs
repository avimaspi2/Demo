// Find all the pairs of types we want to test
ArrayList vars = new ArrayList();
ArrayList types = new ArrayList();
vars.Add("short");	types.Add("byte");
vars.Add("int");	types.Add("short");
vars.Add("long");	types.Add("int");
vars.Add("float");	types.Add("long");
vars.Add("double");	types.Add("float");
vars.Add("int");	types.Add("byte");
vars.Add("long");	types.Add("short");
vars.Add("float");	types.Add("int");
vars.Add("double");	types.Add("long");
vars.Add("long");	types.Add("byte");
vars.Add("float");	types.Add("short");
vars.Add("double");	types.Add("int");
vars.Add("float");	types.Add("byte");
vars.Add("double");	types.Add("short");
vars.Add("double");	types.Add("byte");

CxList allConditions = Find_Conditions();
CxList castExpr = Find_CastExpr();
CxList assignExpr = Find_AssignExpr();

// Get the conditions (that will be a type of sanitizer)
CxList assignExpressions = All.GetByAncs(assignExpr);

CxList conditions = (All - assignExpressions).GetByAncs(allConditions);

// All casting places in the code
CxList inCast = All.GetByAncs(castExpr) - Find_Dead_Code_Contents();

// Look at all the types pairs that we want to compare 
for (int i = 0; i < vars.Count; i++)
{
	// The variable
	CxList varInCast = inCast.FindByType((string) vars[i]);
	// The type
	CxList typeInCast = inCast.FindByType(typeof(TypeRef)).FindByShortName((string) types[i]);
	// Do they clash?
	CxList clash = varInCast.GetFathers() * typeInCast.GetFathers();
	// If so, get the variable
	CxList problems = varInCast.GetByAncs(clash);
	// Leave only results that are not in a condition
	result.Add(problems - (problems * conditions));
}


// Do the same with Real Literals, casted as numbers 
for (int i = 0; i < 4; i++)
{
	// The number
	CxList numInCast = inCast.FindByType(typeof(RealLiteral));
	// The type
	CxList typeInCast = inCast.FindByType(typeof(TypeRef)).FindByShortName((string) types[i]);
	// Do they clash?
	CxList clash = numInCast.GetFathers() * typeInCast.GetFathers();
	// If so, get the variable
	CxList problems = numInCast.GetByAncs(clash);
	// Leave only results that are not in a condition
	result.Add(problems - (problems * conditions));
}


/// Case 2:
// Find a sqrt affected by input that is casted
// Make sure that if there is a checkon the sqrt parameter, then it is OK
CxList sqrt = All.FindByMemberAccess("Math.sqrt").GetByAncs(inCast);
CxList sqrtParam = All.GetParameters(sqrt, 0);
CxList paramInCondition = All.GetByAncs(allConditions).FindAllReferences(sqrtParam);

sqrtParam -= sqrtParam.FindAllReferences(paramInCondition);
CxList input = Find_Inputs();

result.Add(sqrtParam.DataInfluencedBy(input));

/// Remove dead code
result -= Find_Dead_Code_Contents();