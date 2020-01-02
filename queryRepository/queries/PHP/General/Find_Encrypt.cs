CxList methods = Find_Methods();
//the following methods are a part of PHP and used as hash functions:
List < String > list = new List<String>(){ "md5", "sha1", "sha256", "sha512", "sha384", "hash", "hash_hmac", "hash_file",
		// Mcrypt is a part of PHP:
		"mcrypt_generic", "mcrypt_encrypt" };
CxList encrypt = methods.FindByShortNames(list, false); 
encrypt.Add(Find_Kohana_Encrypt());

result = encrypt;