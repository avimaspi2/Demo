// Integer as sanitizer
result = Find_Integers();

// Hash as sanitizer
CxList HashSanitise = 
	All.FindByMemberAccess("MD5CryptoServiceProvider.ComputeHash", false) + 
	All.FindByMemberAccess("RSACryptoServiceProvider.EncryptValue", false) + 
	All.FindByMemberAccess("RSACryptoServiceProvider.SignData", false) + 
	All.FindByMemberAccess("RSACryptoServiceProvider.SignHash", false) + 
	All.FindByMemberAccess("DESCryptoServiceProvider.GetHashCode", false) +
	Find_Methods().FindByShortName("md5", false);

result.Add(HashSanitise);
	
//Methods that cut the flow of data
CxList getMethod = All.FindByMemberAccess(".get", false) + All.FindByMemberAccess(".getvalue", false) ;
CxList removeMethod = All.FindByMemberAccess(".remove", false);

CxList dataStractureGet = 
	//.get
	getMethod.FindByMemberAccess("Collection.get", false) + 
	getMethod.FindByMemberAccess("List.get", false) + 
	getMethod.FindByMemberAccess("Array.getvalue", false) + 
	//.remove
	removeMethod.FindByMemberAccess("Collection.remove", false) + 
	removeMethod.FindByMemberAccess("List.remove", false) + 
	removeMethod.FindByMemberAccess("Array.remove", false);

result.Add(All.GetParameters(dataStractureGet));