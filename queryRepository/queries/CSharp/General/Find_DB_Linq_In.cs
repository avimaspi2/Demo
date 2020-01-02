// All linq methods ( where, select,...)
CxList linqMethods = Find_DB_Linq_Full();

// Get the target of linq methods
CxList linqMethodsTarget = linqMethods.GetTargetOfMembers();

result = linqMethodsTarget;

int countPrev = -1;
int counter = 0;
while (result.Count > countPrev && counter++ < 100)
{
	countPrev = result.Count;
	result.Add(result.GetTargetOfMembers());
}
// Get all paramenter used in linq methods
CxList paramInLinqMethod = All.GetParameters(linqMethods);

result.Add(paramInLinqMethod);

result -= All.FindByName("*LambdaLinq*");
result -= result.DataInfluencingOn(result);