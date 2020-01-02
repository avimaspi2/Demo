/// <summary>
/// The following query will find Insecure Symmetric Encryption Cipher Mode in the Crypto++ library
/// </summary>

// all object creations
CxList objectCreations = Find_ObjectCreations();

// symetric methods that perform encryption and / or decryption
List < string > symetricMethods = new List<string>{
		"Encrypt",
		"ProcessData"
};
// get all the symmetric ciphers instance that perform encryption or decryption
CxList symetricCipherRef = Find_Methods().FindByShortNames(symetricMethods, false).GetTargetOfMembers();

// symetric constructors that perform encryption and / or decryption
List < string > symetricConstructors = new List<string>{
	"AuthenticatedEncryptionFilter",
	"AuthenticatedDecryptionFilter",
	"StreamTransformationFilter"
};
// get all the symetric instances from symetricConstructors
symetricCipherRef.Add(All.GetParameters(objectCreations.FindByShortNames(symetricConstructors), 0));

// find all symmetric ciphers declarators
CxList symetricCipherDeclarators = objectCreations.FindByShortNames(new List<string>{"Encryption", "Decryption"});

// get bad ciphers
CxList badCipherModes = symetricCipherDeclarators.FindByName("OFB_Mode.*");
badCipherModes.Add(symetricCipherDeclarators.FindByName("ECB_Mode.*"));
badCipherModes.Add(symetricCipherDeclarators.FindByName("OFB_Mode.*"));
badCipherModes.Add(symetricCipherDeclarators.FindByName("CFB_Mode.*"));
badCipherModes.Add(symetricCipherDeclarators.FindByName("CBC_Mode.*"));
badCipherModes.Add(symetricCipherDeclarators.FindByName("CTR_Mode.*"));

// get all the insecure symmetric encryption cipher modes
result = symetricCipherRef.DataInfluencedBy(badCipherModes.GetAssignee());