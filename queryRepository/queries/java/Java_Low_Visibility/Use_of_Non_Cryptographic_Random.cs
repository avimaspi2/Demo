CxList random = Find_Random();

// SecureRandom is not vulnerable
random -= random.DataInfluencedBy(All.FindByType("SecureRandom"));
result = random;