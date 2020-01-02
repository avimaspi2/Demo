// We can find only StringLiterals in connection string parameter.
// If the connection string is concatenation of string,
// we can not know if the string variable is in the password part of the text or in another part.
// The connection strings look like this:
// 		MyUserName:AdminPassword@tcp(127.0.0.1:3306)/helloDB
CxList strLiterals = Find_Strings();
strLiterals -= strLiterals.FindByName("");
result = strLiterals.GetParameters(Find_Members_Database(), 1);