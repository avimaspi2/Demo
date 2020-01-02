/*
This query looks for a case where the same password from the user is given twice to the createUser ESAPI command:
 String password = request.getParameter("pwd");
 DefaultUser user = (DefaultUser)ESAPI.authenticator().createUser(name, password, password);

*/

CxList createUser = All.FindByMemberAccess("authenticator.createUser");

CxList inputs = Find_Interactive_Inputs();

// Find password parameters in createUser
CxList param1 = All.GetParameters(createUser, 1).FindByType(typeof(UnknownReference));
CxList param2 = All.GetParameters(createUser, 2).FindByType(typeof(UnknownReference));

// Leave only parameters that are influenced by input
param1 = param1 * param1.DataInfluencedBy(inputs);

// Filter to leave only relevant createParameters
createUser = createUser.FindByParameters(param1).FindByParameters(param2);

// Check for the places that have the same parameter twice
foreach (CxList create in createUser)
{
	CxList p1 = param1.GetParameters(create, 1);
	CxList p2 = param2.GetParameters(create, 2);
	if (p1.FindAllReferences(p2).Count > 0)
	{
		result.Add(p2.Concatenate(p1));
	}
}