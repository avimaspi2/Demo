/**
	This query searches for bad keys used in elliptic curve functions in libsodium
*/

/**** Constant data ****/
long ELLIPTIC_CURVES_MINIMUM_KEY_SIZE = 32;

// Get potential keys
CxList arraysForEllipticCurveKeys = Find_Arrays_By_Size(long.MinValue, ELLIPTIC_CURVES_MINIMUM_KEY_SIZE);


/**** Gathering vulnerabilities ****/
CxList libsodiumPotentialVulnerableArrays = All.NewCxList();
CxList libSodiumEd25519Functions = Find_Methods().FindByShortName("crypto_sign_ed25519_sk_to_seed");
CxList libSodiumEd25519Parameters = All.GetParameters(libSodiumEd25519Functions, 1);
libsodiumPotentialVulnerableArrays.Add(Find_Unknown_References().FindByFathers(libSodiumEd25519Parameters));

libSodiumEd25519Functions = Find_Methods().FindByShortName("crypto_sign_seed_keypair");
libSodiumEd25519Parameters = All.GetParameters(libSodiumEd25519Functions, 0);
libSodiumEd25519Parameters.Add(All.GetParameters(libSodiumEd25519Functions, 1));
libsodiumPotentialVulnerableArrays.Add(Find_Unknown_References().FindByFathers(libSodiumEd25519Parameters));

libSodiumEd25519Functions = Find_Methods().FindByShortName("crypto_box_keypair");
libSodiumEd25519Parameters = All.GetParameters(libSodiumEd25519Functions, 0);
libSodiumEd25519Parameters.Add(All.GetParameters(libSodiumEd25519Functions, 1));
libsodiumPotentialVulnerableArrays.Add(Find_Unknown_References().FindByFathers(libSodiumEd25519Parameters));

libSodiumEd25519Functions = Find_Methods().FindByShortName("crypto_sign_keypair");
libSodiumEd25519Parameters = All.GetParameters(libSodiumEd25519Functions, 0);
libSodiumEd25519Parameters.Add(All.GetParameters(libSodiumEd25519Functions, 1));
libsodiumPotentialVulnerableArrays.Add(Find_Unknown_References().FindByFathers(libSodiumEd25519Parameters));


/**** Joining sanitizers ****/
CxList libsodiumConstants = Find_Unknown_References()
	.FindByShortNames(new List<string>(new string[] {
	"crypto_box_PUBLICKEYBYTES",
	"crypto_box_SECRETKEYBYTES",
	"crypto_sign_PUBLICKEYBYTES",
	"crypto_sign_SECRETKEYBYTES"
	}));


/**** Returning sinks ****/
result = arraysForEllipticCurveKeys
			.InfluencingOnAndNotSanitized(libsodiumPotentialVulnerableArrays, libsodiumConstants)
			.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);