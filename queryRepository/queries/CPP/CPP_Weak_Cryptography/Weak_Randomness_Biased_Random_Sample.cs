// List of Randoms.
CxList randoms = Find_Randoms();

//List of modulus.
CxList binaryExpr = Find_BinaryExpr();
CxList modulus = binaryExpr.GetByBinaryOperator(BinaryOperator.Modulus);

//List of modulus.
CxList floors = Find_Methods().FindByShortName("floor");

//Checks if the randoms are influencing on modulus or floors.
result = randoms.DataInfluencingOn(modulus + floors);