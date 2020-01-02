CxList unknownTest = Find_Unknown_References();
CxList objTest = Find_ObjectCreations();

CxList encrypt = All.FindByShortName("*encrypt*", false);
encrypt.Add(All.FindByMemberAccess("ICryptoTransform.TransformBlock"));

encrypt.Add(All.FindByMemberAccess("ICryptoTransform.TransformFinalBlock"));

encrypt.Add(All.FindByMemberAccess("*.CreateEncryptor"));
encrypt.Add(All.FindByMemberAccess("CryptoStream.Write"));
encrypt.Add(All.FindByMemberAccess("CryptoStream.BeginWrite"));
encrypt.Add(All.FindByMemberAccess("DSA*.CreateSignature"));
encrypt.Add(All.FindByMemberAccess("RSA*.Encrypt*"));
encrypt.Add(All.FindByMemberAccess("MD5*.ComputeHash"));
encrypt.Add(All.FindByMemberAccess("SHA*.ComputeHash"));
encrypt.Add(All.FindByMemberAccess("HMA*.ComputeHash"));
encrypt.Add(All.FindByMemberAccess("ProtectedData.Protect"));

//net core 
/*
*DataProtectionBuilderExtensions.UseCustomCryptographicAlgorithms
*DataProtectionBuilderExtensions.UseCryptographicAlgorithms
*CngCbcAuthenticatedEncryptorConfiguration Class
*ManagedAuthenticatedEncryptorConfiguration Class
*AuthenticatedEncryptorConfiguration Class
*/
CxList cryptoNetCore = All.FindByMemberAccess("IServiceCollection.AddDataProtection")
	.GetMembersOfTarget()
	.FindByShortNames(new List<string> {"UseCustomCryptographicAlgorithms","UseCryptographicAlgorithms"});

CxList allParam =  All.GetParameters(cryptoNetCore);
CxList allParamObj = allParam.FindByType(typeof(ObjectCreateExpr))
	.FindByShortNames(new List<string>{"CngCbcAuthenticatedEncryptorConfiguration", "ManagedAuthenticatedEncryptorConfiguration", "AuthenticatedEncryptorConfiguration"});
allParamObj.Add(All.FindDefinition(allParam - allParamObj).GetAssigner());
/*
*IDataProtector.Protect(IDataProtector, String)
*/
CxList objEphemeralDataProtectionProvider = objTest.FindByType("EphemeralDataProtectionProvider");
CxList provider = unknownTest.FindAllReferences(objEphemeralDataProtectionProvider.GetAssignee());
CxList listCreateProtector = provider.GetMembersOfTarget().FindByShortName("CreateProtector");
CxList protector = unknownTest.FindAllReferences(listCreateProtector.GetAssignee());
CxList protectDataprotection = protector.GetMembersOfTarget().FindByShortName("Protect");

encrypt.Add(allParamObj);
encrypt.Add(cryptoNetCore);
encrypt.Add(protectDataprotection);

/*
*System.Security.Cryptography.AesCcm
*System.Security.Cryptography.AesGcm
*/
CxList objAesGcmCcm = objTest.FindByTypes(new String[]{"AesGcm","AesCcm"}).GetAssignee();
CxList refobjAesGcmCcm = (unknownTest.FindAllReferences(objAesGcmCcm));
CxList encryptAesGcmCcm = refobjAesGcmCcm.GetMembersOfTarget().FindByShortName("Encrypt");
encrypt.Add(encryptAesGcmCcm);

//This is a FilterOutputStream that writes the files into a zip archive
encrypt.Add(All.FindByType("ZipOutputStream", true).FindByType(typeof(UnknownReference)));

result = encrypt;