CxList methods = Find_Methods();
CxList nextRandom = All.FindByMemberAccess("*Random.*");

nextRandom = nextRandom.FindByShortNames(new List<string> {
		"next",
		"nextBoolean",
		"nextBytes",
		"nextDouble",
		"nextFloat",
		"nextGaussian",
		"nextInt",
		"nextLong",
		"doubles",
		/* 	
			ints, doubles and longs refer to the new JAVA 8 Random methods
			these generate streams of random ints, doubles and longs respectively.
			alphanumeric belongs to scala.util.Random and, similarly to
			the previous generates a stream of random alphanumeric characters. 
		*/
		"ints",
		"longs",
		"alphanumeric"});

CxList mathRandom = methods.FindByMemberAccess("Math.random");

CxList secureRandom = methods.FindByMemberAccess("SecureRandom.*");
secureRandom.Add(nextRandom.DataInfluencedBy(secureRandom));

result = nextRandom - secureRandom;
result.Add(mathRandom);