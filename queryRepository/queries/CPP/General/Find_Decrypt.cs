//////////////////////////////////////////////
//			Find_Decrypt
//	Find two things
//		1.Functions that use to encrypt data 
//		2.Objects that are affected from decryption opperation
//////////////////////////////////////////////	 


CxList decrypter = All.FindByShortNames(new List<string> {"*decrypt*", "*unencrypt*"}, false); 
CxList decrypterMethods = All.FindByMemberAccess("CryptoStream.Read*");
decrypter.Add(All.FindByFathers(decrypterMethods));

result = decrypter;