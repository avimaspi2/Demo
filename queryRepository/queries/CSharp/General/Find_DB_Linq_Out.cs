CxList linqMethods = Find_DB_Linq_Full();
CxList linqMethodMembers = linqMethods.GetMembersOfTarget();

result.Add(linqMethodMembers);

int countPost = -1;
int counter = 0;
while (result.Count > countPost && counter++ < 100)
{
	countPost = result.Count;
	result.Add(linqMethodMembers.GetMembersOfTarget());
}

result.Add(linqMethods);

result -= All.FindByShortName("*LambdaLinq*");
result -= result.DataInfluencingOn(result);