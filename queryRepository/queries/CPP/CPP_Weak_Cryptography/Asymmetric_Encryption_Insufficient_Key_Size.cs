/**
 	The following query will find: 
		- Bad usages of RSA implementations with small keys (smaller than 2048 bits/256 Bytes)
		- Bad usages of elliptic curves (like ECDSA and ED25519) implementations with small keys (smaller than 256 bits/32 Bytes)
        
*/

/************** Gathering vulnerabilities ****/
// Botan Library
CxList botanVulnerabilities = Find_Asymmetric_Encryption_Insufficient_Key_Size_Botan();

// Crypto++ Library
CxList cryptoppVulnerabilities = Find_Asymmetric_Encryption_Insufficient_Key_Size_Cryptopp();

// LibSodium Library
CxList libSodiumVulnerabilites = Find_Asymmetric_Encryption_Insufficient_Key_Size_LibSodium();

// OpenSSL Library
CxList openSSLVulnerabilities = Find_Asymmetric_Encryption_Insufficient_Key_Size_OpenSSL();


/************** Joining results ****/
result.Add(botanVulnerabilities);
result.Add(cryptoppVulnerabilities);
result.Add(libSodiumVulnerabilites);
result.Add(openSSLVulnerabilities);