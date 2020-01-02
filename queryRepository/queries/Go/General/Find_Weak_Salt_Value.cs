/*
 * This Query will find all salts used in the given encryption that may be insucure. The
 *  query make sure the salt (the array) has a minimum size of 32 and is randomized
 *  through "crypto/rand", so it will alert all situations where the salt is weak and
 *  used in the given encrypt method.
 *
 * How it works: It will find flows from the array creation to the scrypt encryption,
 *  filtering those (the sanitizers) which the array used as salt is randomized and with
 *  acceptable size.
 *
 * 	package 		- The name of the package 
 *	method 			- The name of the method
*/
if ( param.Length == 2 )
{	
	string package 	= param[0] as string;
	string method 	= param[1] as string;
		
	// Find all possible arrays that may be used as salt;
	CxList possibleSalts = Find_Byte_Slices(); 

	// Find acceptable arrays to be used as Salt;
	CxList sanitizers = Find_Randomized_Slices();

	// Find the encriptions...
	CxList encrypts = All.FindByMemberAccess("\"" + package + "\"." + method);
 
	// ...and focus only in the salt parameter.
	CxList encryptSaltParams = All.GetParameters(encrypts, 1);

	// Alerts are all possible flows from  the weak arrays to the Salt parameter:
	result.Add(possibleSalts
		.InfluencingOnAndNotSanitized(encryptSaltParams, sanitizers)
		.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow));

	// Also add to results all situations where the encryption function has the salt 
	// parameter from a direct cast, where we can't guarantee randomization.
	CxList rankSpecifier = All.FindByType(typeof(RankSpecifier)).FindByFathers(All.FindByType("byte")); 
	CxList paramdeclarators = rankSpecifier.GetAncOfType(typeof(Param)); 
	result.Add(paramdeclarators.GetByAncs(encryptSaltParams));
}
else
{
	cxLog.WriteDebugMessage("Number of parameters should be 2");
}