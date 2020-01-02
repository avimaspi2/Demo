// The Key is the 1st parameter in the following object create classes 
string[] algorithmClasses = {"DESEncryption", "DESDecryption", 
	"DES_EDE2_Encryption", "DES_EDE2_Decryption", "DES_EDE3_Encryption", "DES_EDE3_Decryption", 
	"DES_XEX3_Encryption", "DES_XEX3_Decryption", "AESEncryption", "AESDecryption", 
	"TwofishEncryption", "TwofishDecryption", "BlowfishEncryption", "BlowfishDecryption", 
	"Crypto.DES.Encryption", "Crypto.DES.Decryption",
	"Crypto.DES_EDE2.Encryption", "Crypto.DES_EDE2.Decryption",
	"Crypto.DES_EDE3.Encryption", "Crypto.DES_EDE3.Decryption",
	"Crypto.DES_XEX3.Encryption", "Crypto.DES_XEX3.Decryption",
	"CryptoPP.AES.Encryption", "CryptoPP.AES.Decryption",
	"CryptoPP.Twofish.Encryption", "CryptoPP.Twofish.Decryption",
	"CryptoPP.Blowfish.Encryption", "CryptoPP.Blowfish.Decryption",
	};
CxList objectsCryptopp = All.FindByTypes(algorithmClasses, true);
CxList objectKeyParam = All.GetParameters(objectsCryptopp, 0);

// List of cryptopp classes that provide encryption\decryption interfaces
string[] cryptoppClasses = {"CCM.Encryption", "CCM.Decryption",
	"EAX.Encryption", "EAX.Decryption", "GCM.Encryption", "GCM.Decryption",
	"CBC_Mode.Encryption", "CBC_Mode.Decryption", "AES.Encryption", "AES.Decryption",
	"ECB_Mode.Encryption", "ECB_Mode.Decryption", "OFB_Mode.Encryption", "OFB_Mode.Decryption",
	"ChaCha.Encryption", "ChaCha.Decryption", "HC128.Encryption", "HC128.Decryption",
	"HC256.Encryption", "HC256.Decryption", "Rabbit.Encryption", "Rabbit.Decryption",
	"RabbitWithIV.Encryption", "RabbitWithIV.Decryption", "Salsa20.Encryption", "Salsa20.Decryption",
	"Sosemanuk.Encryption", "Sosemanuk.Decryption", "PanamaCipher.Encryption", "PanamaCipher.Decryption",
	"CFB_Mode.Encryption", "CFB_Mode.Decryption"};
CxList cryptoppRefs = Find_Unknown_References().FindByTypes(cryptoppClasses);

// Key is the 1st parameter in the following methods
List<string> setKeysFunctions = new List<string> {"SetKey", "SetKeyWithRounds", "SetKeyWithIV"};
CxList setKeysMethods = cryptoppRefs.GetMembersOfTarget().FindByShortNames(setKeysFunctions);
CxList keyParam = All.GetParameters(setKeysMethods, 0);

result = objectKeyParam;
result.Add(keyParam);
result -= Find_Parameters();