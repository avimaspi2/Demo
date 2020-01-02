CxList methodInvoke = Find_Methods();
CxList allRequireRefs = Find_Require("node-random");
allRequireRefs.Add(Find_Require("random-js"));
allRequireRefs.Add(Find_Require("unique-random-range"));
allRequireRefs.Add(Find_Require("chance"));
allRequireRefs.Add(Find_Require("Math"));

CxList allInfluByRequire = methodInvoke.DataInfluencedBy(allRequireRefs);

result = allInfluByRequire.FindByShortNames(new List<string> {
		"getRandomInt",
		"integers",
		"integer",
		"strings",
		"quota",
		"numbers",
		"sequence",
		"sequences",
		"uuid",
		"fraction",
		"Random.bool",
		"random"
		});