// Find all seeds used for PRNG's:
// * setSeed commands and parameters
// * Relevant parameter of the constructor
CxList setSeed = All.FindByMemberAccess("Random.setSeed");
result = All.GetParameters(setSeed);