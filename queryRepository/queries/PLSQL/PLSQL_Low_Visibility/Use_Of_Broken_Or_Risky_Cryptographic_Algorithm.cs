CxList algorithm = 
	All.FindByMemberAccess("DBMS_CRYPTO.ENCRYPT_DES", false) + 
	All.FindByMemberAccess("DBMS_CRYPTO.DES_CBC_PKCS5", false);

CxList use = 
	All.FindByMemberAccess("DBMS_CRYPTO.ENCRYPT", false) + 
	All.FindByMemberAccess("DBMS_CRYPTO.DECRYPT", false);


result = algorithm.DataInfluencingOn(use) * algorithm;