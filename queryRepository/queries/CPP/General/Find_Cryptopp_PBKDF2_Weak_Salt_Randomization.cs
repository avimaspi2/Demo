/**
	The following query will find all usages of salts without good randomization being used in 
		Cryptopp implementation of PBKDF2 algorithm. For this implementation the good practice
		requires the usage of GenerateBlock before the PKCS5_PBKDF2_HMAC.DeriveKey execution. 
**/

// Find all possible salt variables
CxList possibleSalts = Find_Arrays();
possibleSalts.Add(Find_Unknown_References().FindByAssignmentSide(CxList.AssignmentSide.Left));
possibleSalts.Add(All.FindByType(typeof(Declarator)));

CxList generateBlockMethods = Find_Methods().FindByShortName("GenerateBlock");
CxList generateBlockMethodsParameters = All.GetParameters(generateBlockMethods, 0);
CxList generateBlock_buf_first_parameters_references = All.FindAllReferences(generateBlockMethodsParameters);
CxList cryptoppSanitizers = generateBlock_buf_first_parameters_references;


CxList cryptoppPBKDF2Executions = Find_Cryptopp_PBDKF2();
CxList salts = All.GetParameters(cryptoppPBKDF2Executions, 5);
CxList cryptoppSaltArrays = Find_Unknown_References().FindByFathers(salts);

// Return the flow from variable declaration until the weak salt usage:
result = possibleSalts.InfluencingOnAndNotSanitized(cryptoppSaltArrays, cryptoppSanitizers);