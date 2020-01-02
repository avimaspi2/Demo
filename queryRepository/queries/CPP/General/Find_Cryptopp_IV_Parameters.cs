//CryptoPP

List<string> setIVFunctions = new List<string> {"SetKeyWithIV"};
		
CxList setIVMethods = Find_Methods().FindByShortNames(setIVFunctions);
result = All.GetParameters(setIVMethods, 2);
result -= Find_Parameters();