CxList inputs = Find_Inputs();
CxList regex = Find_Regex();

CxList goodRegexes = 
	regex.FindByName(@"/^*").FindByName(@"*$/") +
	regex.FindByName(@"^*").FindByName(@"*$/") +
	regex.FindByName(@"/^*").FindByName(@"*$") +
	regex.FindByName(@"!^*").FindByName(@"*$") +
	regex.FindByName(@"s/^*").FindByName(@"*$/") +
	regex.FindByName(@"s/^*").FindByName(@"*$") +
	regex.FindByName(@"{^*").FindByName(@"*$/") +
	regex.FindByName(@"{^*").FindByName(@"*$") +
	regex.FindByName(@"m/^*").FindByName(@"*$/") +
	regex.FindByName(@"m/^*").FindByName(@"*$") +
	regex.FindByName(@"tr/^*").FindByName(@"*$/") +
	regex.FindByName(@"tr/^*").FindByName(@"*$") +
	regex.FindByName(@"/^*").FindByName(@"*$/g") +
	regex.FindByName(@"^*").FindByName(@"*$/g") +
	regex.FindByName(@"s/^*").FindByName(@"*$/g") +
	regex.FindByName(@"{^*").FindByName(@"*$/g") +
	regex.FindByName(@"m/^*").FindByName(@"*$/g") +
	regex.FindByName(@"tr/^*").FindByName(@"*$/g"); // all possible permutations

regex -= goodRegexes;

// Regex in a ~= or !~ operation
CxList regexInBinary = regex.GetAncOfType(typeof(BinaryExpr));

// Find regexes that do not start and end with /^ and $/
result = regexInBinary.DataInfluencedBy(inputs);