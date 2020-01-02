// Find methods that encrypt or hash data

CxList methods = Find_Methods();
CxList use = All.NewCxList();
CxList create = All.NewCxList();

//1. Find heuristics encryption methods 

// crypt is a PHP function used to encrypt strings,
// and all variables labelled crypt(ed) are considered safe, as well as DBMS_CRYPTO as output
CxList heuristicsEncrypt = methods.FindByShortName("crypt*", false); 
heuristicsEncrypt.Add(methods.FindByShortName("CipherStream"));	
heuristicsEncrypt.Add(methods.FindByShortName("createCipher"));	
//Remove the heuristics decrypt mwthods
heuristicsEncrypt -= heuristicsEncrypt.FindByShortName("*decrypt*");
//Remove forge.cipher.createCipher - this is not an encryption method
heuristicsEncrypt -= All.FindByName("forge.cipher.createCipher");

//2. Find CryptoJS encryption methods 

//Progressive Hashing
use = methods.FindByShortName("finalize");

create = All.FindByName("CryptoJS.algo.MD5.create");
create.Add(All.FindByName("CryptoJS.algo.SHA1.create"));
create.Add(All.FindByName("CryptoJS.algo.SHA256.create"));
create.Add(All.FindByName("CryptoJS.algo.SHA512.create"));
create.Add(All.FindByName("CryptoJS.algo.SHA3.create"));
create.Add(All.FindByName("CryptoJS.algo.RIPEMD160.create"));

//Progressive HMAC Hashing
create.Add(All.FindByName("CryptoJS.algo.HMAC.create"));

//Progressive Ciphering
create.Add(All.FindByName("CryptoJS.algo.AES.createEncryptor"));
create.Add(All.FindByName("CryptoJS.algo.DES.createEncryptor"));
create.Add(All.FindByName("CryptoJS.algo.TripleDES.createEncryptor"));
create.Add(All.FindByName("CryptoJS.algo.Rabbit.createEncryptor"));
create.Add(All.FindByName("CryptoJS.algo.RC4.createEncryptor"));
create.Add(All.FindByName("CryptoJS.algo.RC4Drop.createEncryptor"));

//Hashers
CxList CryptoJSEncrypt = Find_Members("CryptoJS.MD5", methods);

CryptoJSEncrypt.Add(Find_Members("CryptoJS.SHA1", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.SHA256", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.SHA512", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.SHA3", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.RIPEMD160", methods));

//HMAC
CryptoJSEncrypt.Add(Find_Members("CryptoJS.HmacMD5", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.HmacSHA1", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.HmacSHA256", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.HmacSHA512", methods));

//Ciphers
CryptoJSEncrypt.Add(Find_Members("CryptoJS.AES.encrypt", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.DES.encrypt", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.TripleDES.encrypt", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.Rabbit.encrypt", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.RC4.encrypt", methods));
CryptoJSEncrypt.Add(Find_Members("CryptoJS.RC4Drop.encrypt", methods));

CryptoJSEncrypt.Add(use.DataInfluencedBy(create).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//3. Find Cipherstream encryption methods 

CxList CipherstreamEncrypt = Find_Members("cipherstream.CipherStream", methods);
CipherstreamEncrypt.Add(Find_Members("cipherstream.hmac", methods));

//4. Find Forge encryption methods 

//CIPHER

CxList ForgeEncrypt = All.NewCxList();

use = methods.FindByShortName("finish");

create = All.FindByName("forge.cipher.createCipher");

//RC2
create.Add(All.FindByName("forge.rc2.createEncryptionCipher"));
ForgeEncrypt.Add(use.DataInfluencedBy(create).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//RSA
use = methods.FindByShortName("encrypt");
create = All.FindByName("forge.pki.rsa.generateKeyPair.publicKey");
ForgeEncrypt.Add(use.DataInfluencedBy(create).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//Message Digests
use = methods.FindByShortName("digest");
create = All.FindByName("forge.md.sha1.create");
create.Add(All.FindByName("forge.md.sha256.create"));
create.Add(All.FindByName("forge.md.sha384.create"));
create.Add(All.FindByName("forge.md.sha512.create"));
create.Add(All.FindByName("forge.md.md5.create"));
create.Add(All.FindByName("forge.hmac.create"));

ForgeEncrypt.Add(use.DataInfluencedBy(create).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//5. Find KJUR.crypto.MessageDigest encryption methods

create = All.FindByName("KJUR.crypto.MessageDigest");
CxList KJUREncrypt = use.DataInfluencedBy(create).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

//6. Find SJCL encryption methods 
CxList SJCLEncrypt = Find_Members("sjcl.encrypt", methods);

//Combines everything we've found so far
result = heuristicsEncrypt;
result.Add(CryptoJSEncrypt);
result.Add(CipherstreamEncrypt);
result.Add(KJUREncrypt);
result.Add(ForgeEncrypt);
result.Add(SJCLEncrypt);