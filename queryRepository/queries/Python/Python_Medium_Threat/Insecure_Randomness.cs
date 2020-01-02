// https://docs.python.org/2/library/random.html
// The pseudo-random generators of this module should not be used for security purposes. 
// Use os.urandom() or SystemRandom if you require a cryptographically secure pseudo-random number generator.
CxList randomMethods = Find_Methods_By_Import("random", 
	new string[]{"random", "uniform", "randint", "randrange", "triangular", "betavariate", "expovariate", "gammavariate",
		"gauss", "lognormvariate", "normalvariate", "vonmisesvariate", "paretovariate", "weibullvariate"});
//	.betavariate(alpha, beta), .expovariate(lambd), .gammavariate(alpha, beta), .gauss(mu, sigma), .lognormvariate(mu, sigma)
//	.normalvariate(mu, sigma), .vonmisesvariate(mu, kappa), .paretovariate(alpha), .weibullvariate(alpha, beta)
// 	generate random numbers according to a defined distribution and so are even weaker than other generators.

result = randomMethods;