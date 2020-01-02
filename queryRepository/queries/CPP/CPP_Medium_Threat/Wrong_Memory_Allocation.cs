//	Wrong Memory Allocation
//  -----------------------
//  In this query we look for calls to malloc with sizeof, where there
//  is no correct multiplication of the parameter by ist size.
///////////////////////////////////////////////////////////////////////

// Find all mallocs
CxList mallocs = Find_Methods().FindByShortName("malloc");
// Find the sizeof functions that are direct parameters of malloc (no sizeof*somthing)
CxList sizeofInMalloc = Find_Methods().GetParameters(mallocs).FindByShortName("sizeof");

// Get the sizeof's parameters
CxList sizeofParams = All.GetParameters(sizeofInMalloc);

// Filter all the primitives (sizeof(int) is OK) and the pointers (sizeof(*p) is
// usually OK)
sizeofParams -= sizeofParams.FindByShortName("bool");
sizeofParams -= sizeofParams.FindByShortName("short");
sizeofParams -= sizeofParams.FindByShortName("char");
sizeofParams -= sizeofParams.FindByShortName("int");
sizeofParams -= sizeofParams.FindByShortName("long");
sizeofParams -= sizeofParams.FindByShortName("float");
sizeofParams -= sizeofParams.FindByShortName("double");
sizeofParams -= sizeofParams.FindByShortName("Pointer");

// Filter any types
CxList sparams = sizeofParams;
foreach (CxList sop in sparams)
{
	CSharpGraph g = sop.data.GetByIndex(0) as CSharpGraph;
	CxList types = All.FindByType(g.FullName) + 
		All.FindByType(g.ShortName) - 
		sizeofParams - 
		All.FindByShortName(g.FullName) - 
		All.FindByShortName(g.ShortName);
	if (types.Count > 0) 
	{
		sizeofParams -= sop;
	}
}

// Find the relevant sizeof's methods
result = sizeofParams.GetAncOfType(typeof(MethodInvokeExpr));