//  This query returns all creations of random numbers and strings by the Random class
// or by the default PRNG of Ruby (Random::DEFAULT)
CxList weakRandom = All.FindByType("Random");
CxList randomMembers = weakRandom.GetMembersOfTarget();	// all methods of Random objects
List<string> methodNames = new List<string>{"rand", "bytes"};	// methods for generating random numbers and strings
CxList randomValue = randomMembers.FindByShortNames(methodNames);

CxList methods = Find_Methods();
CxList randomMethods = methods.FindByShortNames(methodNames);

CxList defaultRandomMethods = randomMethods - randomMethods.FindByMemberAccess("*.*"); // methods of Random::DEFAULT

CxList declarators = weakRandom.FindByType(typeof(ObjectCreateExpr));	// instances of prng=Random.new and prng=Random.new(seed)

result = declarators.InfluencingOn(randomValue);	// add: prng.rand prng.rand(val) etc.
result.Add(defaultRandomMethods);					// add: rand, rand(val) etc.
result.Add(declarators.GetMembersOfTarget());		// add: Random.new.bytes(size) Random.new.rand(val) etc.