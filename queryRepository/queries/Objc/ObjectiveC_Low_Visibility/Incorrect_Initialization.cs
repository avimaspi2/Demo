/* Often-forgotten initialization process (only alloc, no inits).

MyClass* myInstance4 = [MyClass alloc];

Instead of correct cases such as:

	MyClass* myInstance1 = [[MyClass alloc] init];
	MyClass* myInstance2 = [[MyClass alloc] initWithNumber: 1];
	MyClass* myInstance3 = [[MyClass alloc] initWithNumber: 1 andString: "hello"];

*/

CxList alloc = All.FindByMemberAccess("*.alloc");
CxList relevantAlloc = alloc - alloc.GetMembersOfTarget().GetTargetOfMembers();

relevantAlloc = relevantAlloc.FindByRegex(@"alloc\];");
result = relevantAlloc - relevantAlloc.FindByRegex(@"\[super");