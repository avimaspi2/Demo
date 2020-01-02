// get all passwords
CxList pass = Find_All_Passwords();

// get all string types
CxList strings = Find_String_Types();

// return intersect of passwords and string types
result = pass * strings;