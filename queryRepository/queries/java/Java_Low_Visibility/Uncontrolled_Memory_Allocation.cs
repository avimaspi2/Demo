// Get all objects that allocate memory at startup
CxList memAlloc = All.FindByTypes(new string[] {
	"ArrayList",
	"AttributeList",
	"ConcurrentHashMap",
	"HashMap",
	"HashSet",
	"Hashtable",
	"IdentityHashMap",
	"LinkedHashMap",
	"LinkedHashSet",
	"PrinterStateReasons",
	"RoleList",
	"RoleUnresolvedList",
	"Vector",
	"WeakHashMap"});

memAlloc = Find_ObjectCreations().GetByAncs(memAlloc);

CxList methods = Find_Methods();
memAlloc.Add(methods.FindByMemberAccess("Collections.nCopies"));
memAlloc.Add(methods.FindByMemberAccess("ArrayList.ensureCapacity"));
memAlloc.Add(methods.FindByMemberAccess("ConcurrentHashMap.newKeySet"));
memAlloc.Add(methods.FindByMemberAccess("Arrays.copyOf"));

// All inputs + maxInt value + random values
CxList inputs = Find_Inputs();
inputs.Add(All.FindByMemberAccess("Integer.MAX_VALUE"));
inputs.Add(All.FindByMemberAccess("Random.Next*", false));
inputs.Add(All.FindByMemberAccess("Math.random", false));
inputs.Add(Get_ESAPI().FindByMemberAccess("Randomizer.*")); //ESAPI

CxList sanitizers = All.FindByTypes(new string [] {"bool","boolean"});

CxList AllocParameters = All.GetParameters(memAlloc);
CxList integersAbstractValues = AllocParameters.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue);
IAbstractValue intervalUpToHundredThousand = new IntegerIntervalAbstractValue(null, 100000);
CxList validIntervals = integersAbstractValues.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(intervalUpToHundredThousand));
sanitizers.Add(validIntervals);


// Inputs influencing memory allocations.
result = memAlloc.InfluencedByAndNotSanitized(inputs, sanitizers);