// Find all strings
CxList strings = Find_Strings();
// find all inputs
CxList inputs = Find_Inputs();

// Get all the relevant digest algorithms
CxList digestStrings = 
	strings.FindByShortName("\"SHA*") + 
	strings.FindByShortName("\"MD2*") +
	strings.FindByShortName("\"MD5*");

// All the update commands
CxList update = All.FindByMemberAccess("MessageDigest.update");


// The result - any update of the relevant algorithms, influenced by any input
result = update.DataInfluencedBy(digestStrings).DataInfluencedBy(inputs);