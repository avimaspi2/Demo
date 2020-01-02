CxList methods = Find_Methods();
CxList unknownReference = Find_UnknownReference();
CxList memberAccesses = Find_MemberAccesses();

// Sanitized
CxList sanitized = All.NewCxList();
CxList secureRandomNextBytes = methods.FindByMemberAccess("SecureRandom.nextBytes");
sanitized.Add(unknownReference.GetParameters(secureRandomNextBytes, 0));
sanitized.Add(unknownReference.FindByType("PBEKeySpec"));

CxList sanitizedKeys = All.FindDefinition(sanitized);

// Get Cipher.init in encrypt mode
CxList cipherInitMethods = methods.FindByMemberAccess("Cipher.init");
CxList paramCipherInitMethod = memberAccesses.FindByMemberAccess("Cipher.ENCRYPT_MODE").GetParameters(cipherInitMethods, 0);
cipherInitMethods = methods.FindByParameters(paramCipherInitMethod);

CxList keys = All.NewCxList();
keys.Add(unknownReference.GetParameters(cipherInitMethods, 1));

CxList keysToRemove = keys.DataInfluencedBy(sanitizedKeys);
keys -= keysToRemove;

result = keys;