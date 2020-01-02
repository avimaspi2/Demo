/**
 Find weak encryption from Botan 2.5
 **/


CxList Methods = Find_Methods();
CxList ObjCreate = Find_ObjectCreations();
CxList Strings = Find_Strings();
CxList VarDeclStmt = Find_VariableDeclStmt();
CxList Declarator = Find_Declarators();
CxList UnknownReference = Find_UnknownReference();


// Vulnerable hashes
CxList BotanHashCreate = Methods.FindByName("Botan.HashFunction.create");
string[] VulnerableHashes = new string[]{"SHA-1", "SHA-160", "SHA1", "RIPEMD-160", "MD4", "MD5"};

foreach (string VulnHash in VulnerableHashes)
{
	CxList Strings_VulnHash = Strings.FindByShortName(VulnHash);
	CxList BotanHashCreateVulnHash = BotanHashCreate.FindByParameters(Strings_VulnHash);
	CxList BotanHashFunctionVulnHashObjCreate = ObjCreate.FindByParameters(BotanHashCreateVulnHash);
	CxList BotanHashFunctionVulnHashDeclarator = Declarator.GetByAncs(BotanHashFunctionVulnHashObjCreate.GetAncOfType(typeof(VariableDeclStmt)));
	
	foreach (CxList DeclVulnHash in BotanHashFunctionVulnHashDeclarator)
	{
		CxList BotanVulnHash = UnknownReference.FindAllReferences(DeclVulnHash);
		CxList BotanVulnHashMembers = BotanVulnHash.GetMembersOfTarget();
		
		CxList BotanVulnHashUpdate = BotanVulnHashMembers.FindByShortName("update");
		CxList BotanVulnHashFinal = BotanVulnHashMembers.FindByShortName("final");
		CxList BotanVulnHashProcess = BotanVulnHashMembers.FindByShortName("process");
		
		result.Add(BotanVulnHashUpdate.ConcatenateAllPaths(BotanVulnHashFinal, false));
		result.Add(BotanVulnHashProcess);
	}
}