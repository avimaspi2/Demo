/// <summary>
/// The following query will find Insecure Symmetric Encryption Cipher Mode in the Botan library
/// </summary>

// find all create cipher methods
// Cipher Modes (https://botan.randombit.net/manual/cipher_modes.html)
CxList createCipherModeMethods = Find_Methods().FindByName("Botan.Cipher_Mode.create");
createCipherModeMethods.Add(Find_Methods().FindByName("Botan.get_cipher_mode"));
// Stream Ciphers (https://botan.randombit.net/manual/stream_ciphers.html)
createCipherModeMethods.Add(Find_Methods().FindByName("Botan.StreamCipher.create"));
// Pipe/Filter Message Processing (https://botan.randombit.net/manual/filters.html)
createCipherModeMethods.Add(Find_Methods().FindByName("Botan.get_cipher"));
createCipherModeMethods.Add(Find_ObjectCreations().FindByName("Botan.StreamCipher_Filter"));

// get the cipher algoritms used (example: "AES-128/CCM", "Twofish/CFB(8)")
CxList cipherAlgorithm = Find_String_Literal().GetParameters(createCipherModeMethods, 0);

// get insecure cipher modes (ECB, CFB, CBC, CTR and OFB)
CxList insecureCipherModes = cipherAlgorithm.FindByShortNames(new List<string>{		
	"*ECB*", // Electronic Codebook Mode (ECB)
	"*CFB*", // Cipher Feedback Mode (CFB)
	"*CBC*", // Cipher Block Chaining (CBC)
	"*CTR*", // Counter Mode (CTR)
	"*OFB*", // Output Feedback Mode (OFB)
});
// add all Block Cipher (https://botan.randombit.net/manual/block_cipher.html) as insecure cipher modes
insecureCipherModes.Add(Find_Methods().FindByName("Botan.BlockCipher.create"));

// symetric methods that perform encryption and / or decryption
List < string > symetricMethods = new List<string>{
	// Cipher Modes | Stream Ciphers | Block Ciphers
	"encrypt",
	"decrypt",
	"encipher",
	"finish",
	"cipher",
	"cipher1",
	// Pipe/Filter Message Processing (https://botan.randombit.net/manual/filters.html)
	"process_msg",
	"start_msg",
	"write",
	"send",
	"end_msg"
};

// get all the symmetric ciphers instance that perform encryption or decryption
CxList symetricCipherRef = Find_Methods().FindByShortNames(symetricMethods, false).GetTargetOfMembers();

// get all the insecure symmetric encryption cipher modes
result = symetricCipherRef.DataInfluencedBy(insecureCipherModes);