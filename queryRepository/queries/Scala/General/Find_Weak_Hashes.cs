// Find usage of weak ciphers by dynamic methods

CxList strings = Find_Strings();
CxList methods = Find_Methods();

//Look for standard and not standard names for weak hash names 
//This list contains all weak hash algorithms <digest>
List<string> weakHashNames = new List<string> {"MD2","MD5","SHA-1","SHA1","SHA","SHA1PRNG"};

//OAEPWith<digest>AndMGF1Padding
weakHashNames.AddRange(new List<string> {"OAEPWithSHA-1AndMGF1Padding","OAEPWithMD5AndMGF1Padding","OAEPWithMD2AndMGF1Padding","OAEPWithSHA1AndMGF1Padding"});
//Hmac<digest>
weakHashNames.AddRange(new List<string> {"HmacMD2","HmacMD5","HmacSHA-1","HmacSHA1"});
//<digest>withRSA encryption
weakHashNames.AddRange(new List<string> {"MD2withRSA","MD5withRSA","SHA-1withRSA","SHA1withRSA"});
//<digest>withRC5 encryption
weakHashNames.AddRange(new List<string> {"MD2withRC5","MD5withRC5","SHA-1withRC5","SHA1withRC5"});
//<digest>withRC4 encryption
weakHashNames.AddRange(new List<string> {"MD2withRC4","MD5withRC4","SHA-1withRC4","SHA1withRC4"});
//<digest>withRC2 encryption
weakHashNames.AddRange(new List<string> {"MD2withRC2","MD5withRC2","SHA-1withRC2","SHA1withRC2"});
//<digest>withECIES encryption
weakHashNames.AddRange(new List<string> {"MD2withECIES","MD5withECIES","SHA-1withECIES","SHA1withECIES"});
//<digest>withDES encryption
weakHashNames.AddRange(new List<string> {"MD2withDES","MD5withDES","SHA-1withDES","SHA1withDES"});
//<digest>withDESede encryption
weakHashNames.AddRange(new List<string> {"MD2withDESede","MD5withDESede","SHA-1withDESede","SHA1withDESede"});
//<digest>withDESedeWrap encryption
weakHashNames.AddRange(new List<string> {"MD2withDESedeWrap","MD5withDESedeWrap","SHA-1withDESedeWrap","SHA1withDESedeWrap"});
//<digest>withAES encryption
weakHashNames.AddRange(new List<string> {"MD2withAES","MD5withAES","SHA-1withAES","SHA1withAES"});
//<digest>withAESWrap encryption
weakHashNames.AddRange(new List<string> {"MD2withAESWrap","MD5withAESWrap","SHA-1withAESWrap","SHA1withAESWrap"});
//<digest>withARCFOUR encryption
weakHashNames.AddRange(new List<string> {"MD2withARCFOUR","MD5withARCFOUR","SHA-1withARCFOUR","SHA1withARCFOUR"});
//<digest>withBlowfish encryption
weakHashNames.AddRange(new List<string> {"MD2withBlowfish","MD5withBlowfish","SHA-1withBlowfish","SHA1withBlowfish"});

//All previous with "andMGF1"
weakHashNames.AddRange(new List<string> {"MD2withRSAandMGF1","MD5withRSAandMGF1","SHA-1withRSAandMGF1","SHA1withRSAandMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withRC5andMGF1","MD5withRC5andMGF1","SHA-1withRC5andMGF1","SHA1withRC5andMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withRC4andMGF1","MD5withRC4andMGF1","SHA-1withRC4andMGF1","SHA1withRC4andMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withRC2andMGF1","MD5withRC2andMGF1","SHA-1withRC2andMGF1","SHA1withRC2andMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withECIESandMGF1","MD5withECIESandMGF1","SHA-1withECIESandMGF1","SHA1withECIESandMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withDESandMGF1","MD5withDESandMGF1","SHA-1withDESandMGF1","SHA1withDESandMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withDESedeandMGF1","MD5withDESedeandMGF1","SHA-1withDESedeandMGF1","SHA1withDESedeandMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withDESedeWrapandMGF1","MD5withDESedeWrapandMGF1","SHA-1withDESedeWrapandMGF1","SHA1withDESedeWrapandMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withAESandMGF1","MD5withAESandMGF1","SHA-1withAESandMGF1","SHA1withAESandMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withAESWrapandMGF1","MD5withAESWrapandMGF1","SHA-1withAESWrapandMGF1","SHA1withAESWrapandMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withARCFOURandMGF1","MD5withARCFOURandMGF1","SHA-1withARCFOURandMGF1","SHA1withARCFOURandMGF1"});
weakHashNames.AddRange(new List<string> {"MD2withBlowfishandMGF1","MD5withBlowfishandMGF1","SHA-1withBlowfishandMGF1","SHA1withBlowfishandMGF1"});


// Creation of MessageDigest objects
// e.g:  Cipher cipher = Cipher.getInstance("SHA1");
// Also handle use of Cipher class allocation, such as:
//      Cipher c = new Cipher(new CipherSpi(), new Provider(), "RSA/ECB/OAEPWithSHA-1AndMGF1Padding");
List<string> DigestConstructors = new List<string> {"MessageDigest","Cipher"};
CxList objectCreations = Find_Object_Create();
CxList msgDigestCreate = objectCreations.FindByShortNames(DigestConstructors);
CxList msgDigestCreateParams = All.GetParameters(msgDigestCreate);

// Filter by string parameters for weak hashes
CxList msgDigstWeak = msgDigestCreateParams.FindByShortNames(weakHashNames, false);


// Find the actual creation
CxList msgDigest = msgDigstWeak.GetAncOfType(typeof(ObjectCreateExpr));

// Find message digest factory such as:
//      MessageDigest md1 = MessageDigest.getInstance("SHA1");
//      MessageDigest md2 = DigestUtils.getDigest("MD5");
msgDigestCreate.Add(methods.FindByShortName("getInstance"));
msgDigestCreate.Add(methods.FindByMemberAccess("DigestUtils.getDigest"));

// Find all strings containing weak hashes as 
CxList weakKeysStrings = msgDigest;
weakKeysStrings.Add(strings.FindByShortNames(weakHashNames, false));

// Round up all digests initialized by weak keys as string or by another object
CxList DigestInstance = msgDigestCreate.FindByParameters(weakKeysStrings);
DigestInstance.Add(msgDigestCreate.DataInfluencedBy(weakKeysStrings));

result = DigestInstance;