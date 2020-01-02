CxList unknownRefs = Find_UnknownReference();
CxList objCreations = Find_ObjectCreations();
CxList methods = Find_Methods();

CxList insufficientlyRandomValues = Find_Random();
insufficientlyRandomValues.Add(All.GetParameters(insufficientlyRandomValues));

CxList javaxCryptoSinks = All.NewCxList();
// javax.crypto.spec.SecretKeySpec
javaxCryptoSinks.Add(All.GetParameters(objCreations.FindByShortName("SecretKeySpec")));

// javax.crypto.KeyGenerator
javaxCryptoSinks.Add(methods.FindByExactMemberAccess("KeyGenerator.init"));

// javax.crypto.Cipher
javaxCryptoSinks.Add(methods.FindByExactMemberAccess("Cipher.init"));

CxList javaSecuritySinks = All.NewCxList();
// java.security.MessageDigest
javaSecuritySinks.Add(methods.FindByExactMemberAccess("MessageDigest.update"));
javaSecuritySinks.Add(methods.FindByExactMemberAccess("MessageDigest.digest"));

// java.security.Signature
javaSecuritySinks.Add(methods.FindByExactMemberAccess("Signature.initSign"));
javaSecuritySinks.Add(methods.FindByExactMemberAccess("Signature.initVerify"));

// java.security.KeyFactorySpi
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyFactorySpi.engineGeneratePrivate"));
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyFactorySpi.engineGeneratePublic"));

// java.security.KeyFactory
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyFactory.KeyFactory"));
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyFactory.generatePrivate"));
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyFactory.generatePublic"));

// java.security.KeyPairGenerator
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyPairGenerator.initialize"));

CxList sinks = javaxCryptoSinks;
sinks.Add(javaSecuritySinks);

result = sinks.DataInfluencedBy(insufficientlyRandomValues);