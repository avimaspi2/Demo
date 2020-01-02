// Find all passwords that are variables of type String or StringBuilder
CxList allPasswords = Find_All_Passwords(); 
result = allPasswords.FindByType("String", false);
result.Add(allPasswords.FindByType("StringBuilder", false));