/*	Null_Password

	This query looks for password credentials that are not
	validated before being used in a comparison with input.
	More specifically, this query looks for the following
	pattern of operation:
	A) credentials are read from a database;
	B) credentials are not validated (via some comparison);
	C) credentials are compared with user input.
*/

/* 0. Find variables storing passwords */
CxList variables = Find_UnknownReference();
CxList passwords = All_Passwords();
CxList passwordVariables = variables * passwords;


/* 1. Find credentials */
CxList dbOut = NodeJS_Find_DB_Out();
CxList ancs = dbOut.GetAncOfType(typeof(LambdaExpr));
ancs.Add(dbOut.GetAncOfType(typeof(MethodDecl)));
CxList memberAccess = Find_MemberAccesses().GetByAncs(ancs);
CxList credentials = passwordVariables.DataInfluencedBy(memberAccess.DataInfluencedBy(dbOut));

/* 2. Find input passwords */
CxList inputs = NodeJS_Find_Inputs();
CxList inputPasswordVariables = passwordVariables.DataInfluencedBy(inputs);
CxList inputPasswordComparisons = inputPasswordVariables.GetFathers().FindByType(typeof(BinaryExpr));

/* 3. Find where the credentials are compared with the input */
CxList credentialComparisons = credentials.GetFathers().FindByType(typeof(BinaryExpr));
CxList credentialDeciders = credentialComparisons * inputPasswordComparisons;

/* 4. Find credential validators */
CxList credentialValidators = credentialComparisons - credentialDeciders;

/* 5. Decide which credentials are validated */
CxList credentialsInDeciders = credentials.GetByAncs(credentialDeciders);
CxList credentialsInValidators = credentials.GetByAncs(credentialValidators);

/* Note: We can't just do credentialsInDeciders - credentialsInValidators, for
   even when the same variable is in both lists, their ID is different. As such
   we guide ourselves by their defining statements. */

CxList validatedCredentials = All.NewCxList();
CxList definitions = Find_Declarators();

foreach (CxList c in credentialsInDeciders)
{
	CxList cDefinition = definitions.FindDefinition(c);
	if (cDefinition.FindDefinition(credentialsInValidators).Count > 0)
		validatedCredentials.Add(c);
}

result = credentialsInDeciders - validatedCredentials;