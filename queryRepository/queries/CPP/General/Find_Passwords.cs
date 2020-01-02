CxList allPasswords = Find_All_Passwords();
CxList stringAndCharPasswords =  allPasswords.FindByType("std.string") + allPasswords.FindByType("char") +
allPasswords.FindByType("string");
stringAndCharPasswords.Add( allPasswords.FindByAbstractValue(s => s is StringAbstractValue));
result = stringAndCharPasswords;