CxList digestStrings = Find_Hash_Strings();


// All the digest commands
CxList digest = Find_Digest_Commands();
// All the update commands (that are necessary as a salt, so they will be our sanitizer)
CxList update = Find_Update_Commands();
update.Add(update.GetTargetOfMembers());

// And the result - any use of an algorithm without salt
result = digest.InfluencedByAndNotSanitized(digestStrings, update);