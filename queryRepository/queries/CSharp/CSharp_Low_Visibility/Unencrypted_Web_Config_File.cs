/// <summary>
/// This query returns all the web config files that are unencrypted
/// </summary>

CxList webConfigs = Find_Web_Config();

// encrypted values and keys
CxList xmlTokens = webConfigs.FindByAssignmentSide(CxList.AssignmentSide.Left).GetTargetOfMembers();
xmlTokens -= xmlTokens.FindByShortNames(new List<string>{"keyinfo","keyname","encryptionmethod", "encryptedkey", "encrypteddata","cipherdata","ciphervalue"}, false);

// encrypted keynames
xmlTokens = xmlTokens.GetMembersOfTarget();
xmlTokens -= xmlTokens.FindByShortName("configprotectionprovider", false);

result = All.GetClass(xmlTokens);