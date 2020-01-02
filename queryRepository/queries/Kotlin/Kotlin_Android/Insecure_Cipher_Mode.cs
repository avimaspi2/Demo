CxList methods = Find_Methods();
CxList strings = Find_Strings();
CxList unknownReference = Find_UnknownReference();

CxList stringFilter = strings.FindByShortNames(new List<string> {"AES","AES/ECB*", "AES/CBC*"}); 

CxList cipherGetInstance = methods.FindByMemberAccess("Cipher.getInstance");
CxList encryptionStrings = stringFilter.GetParameters(cipherGetInstance, 0);

CxList unknownParameter = unknownReference.GetParameters(cipherGetInstance, 0);
encryptionStrings.Add(stringFilter.DataInfluencingOn(unknownParameter).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));

CxList hmacGetInstance = methods.FindByMemberAccess("Mac.getInstance");
CxList hmacVar = unknownReference.FindAllReferences(hmacGetInstance.GetAncOfType(typeof(Declarator)));
CxList hmacTarget = hmacVar.GetMembersOfTarget();
result = encryptionStrings - hmacTarget.DataInfluencedBy(encryptionStrings).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);