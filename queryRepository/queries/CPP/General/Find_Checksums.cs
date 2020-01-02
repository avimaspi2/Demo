result = Find_Weak_Encryption();

List < string > cs_names = new List<string> {
		"CRC32",				//cryptopp
		"Adler32",				//cryptopp
		"CheckSumMappedFile",	//windows
		"MapFileAndCheckSum"	//windows
		};


CxList methods = Find_Methods();
CxList cs_methods = methods.FindByShortNames(cs_names, false);
cs_methods.Add(methods.FindByMemberAccess("CFastCRC32.Calculate",false));

//for CryptoPP::CRC32/Adler
CxList cs_types = All.FindByTypes(cs_names.ToArray(), false);
cs_types.Add(All.FindByShortName("CFastCRC32"));

result.Add(cs_methods);
result.Add(cs_types);