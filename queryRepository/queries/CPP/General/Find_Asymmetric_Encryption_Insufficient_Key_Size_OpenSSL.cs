/**
	This query searches for keys used in OpenSSL's elliptic curve and RSA functions
*/

/**** Constant data ****/
int RSA_MINIMUM_KEY_SIZE = 2048;

// nid white list 
List<string> safeNidStrings = new List<string>(new string[] {
	"secp256k1","secp384r1", "secp521r1",
	"prime256v1",
	"sect283k1","sect283r1","sect409k1","sect409r1","sect571k1","sect571r1",
	"c2pnb272w1","c2pnb304w1","c2tnb359v1","c2pnb368w1","c2tnb431r1",
	"brainpoolP256r1","brainpoolP256t1","brainpoolP320r1","brainpoolP320t1","brainpoolP384r1","brainpoolP384t1","brainpoolP512r1","brainpoolP512t1"
	});

/**** Gathering vulnerabilities ****/
// Find all RSA key generation methods 
CxList rsaFunctions = Find_Not_In_Range("openssl/rsa.h", "RSA_generate_key_ex", 1, RSA_MINIMUM_KEY_SIZE -1, null);
rsaFunctions.Add(Find_Not_In_Range("openssl/rsa.h", "RSA_generate_multi_prime_key", 1, RSA_MINIMUM_KEY_SIZE -1, null));
rsaFunctions.Add(Find_Not_In_Range("openssl/rsa.h", "RSA_generate_key", 0, RSA_MINIMUM_KEY_SIZE -1, null));


// Find all elliptic curves key generation methods
CxList txt2nidFunctions = Find_Methods().FindByShortName("OBJ_txt2nid");
CxList txt2nidParameters = All.GetParameters(txt2nidFunctions, 0);
CxList txt2nidStrings = Find_String_Literal().FindByFathers(txt2nidParameters);
CxList vulnerableText2nidStrings = txt2nidStrings - Find_String_Literal().FindByShortNames(safeNidStrings);

CxList openSSLCurveFunctions = Find_Methods().FindByShortNames(new List<string>(new string[] {
	"EC_KEY_new_by_curve_name", "EVP_PKEY_CTX_set_ec_paramgen_curve_nid"
	}));

CxList vulnerableECKey = vulnerableText2nidStrings.InfluencingOn(openSSLCurveFunctions);


/**** Returning sinks ****/
result.Add(rsaFunctions);
result.Add(vulnerableECKey);