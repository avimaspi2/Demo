// Find all strings
CxList strings = Find_Strings();
// Get all the relevant digest algorithms
CxList digestStrings = 
	strings.FindByShortName("\"SHA*") + 
	strings.FindByShortName("\"MD2*") +
	strings.FindByShortName("\"MD5*");
// All the digest commands
CxList digest = All.FindByMemberAccess("MessageDigest.digest");
// All the update commands (that are necessary as a salt, so they will be our sanitizer)
CxList update = All.FindByMemberAccess("MessageDigest.update");
update.Add(update.GetTargetOfMembers());

// And the result - any use of an algorithm without salt
result = digest.InfluencedByAndNotSanitized(digestStrings, update);