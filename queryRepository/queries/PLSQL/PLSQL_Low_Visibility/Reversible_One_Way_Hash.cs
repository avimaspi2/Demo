CxList algorithm = 
	All.FindByMemberAccess("DBMS_CRYPTO.HASH_MD4",false) +
	All.FindByMemberAccess("DBMS_CRYPTO.HASH_MD5",false) + 
	All.FindByMemberAccess("DBMS_CRYPTO.HASH_SH1",false);

CxList hash = All.FindByMemberAccess("DBMS_CRYPTO.HASH", false);


result = algorithm.DataInfluencingOn(hash) * algorithm;