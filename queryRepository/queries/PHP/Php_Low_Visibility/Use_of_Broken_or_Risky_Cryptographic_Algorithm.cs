CxList methods = Find_Methods();
CxList strings = Find_Strings();
CxList algos = strings.FindByShortNames(new List<String>(){ "md*", "sha1" });
CxList paramOFHash = All.FindByType(typeof(UnknownReference)) + strings;
paramOFHash = paramOFHash.GetParameters(methods.FindByShortNames(new List<String>()
	{ "hash", "hash_file", "hash_hmac_file", "hash_hmac", "hash_init" }), 0);
	
result.Add(paramOFHash.DataInfluencedBy(algos));
result.Add(paramOFHash * algos);
result.Add(methods.FindByShortNames(new List<String>(){ "md5", "sha1", "sha1_file", "md5_file" }));