CxList random = All.FindByType("Random");
CxList assigned = All.FindByInitialization(random); 
random += All.FindAllReferences(assigned); 
CxList nextRandom = random.GetMembersOfTarget();

nextRandom = 
	nextRandom.FindByShortName("next") + 
	nextRandom.FindByShortName("nextInt") + 
	nextRandom.FindByShortName("nextBoolean") +
	nextRandom.FindByShortName("nextLong");

CxList mathRandom = All.FindByMemberAccess("Math.random");

result = nextRandom + mathRandom;