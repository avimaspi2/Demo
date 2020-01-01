// Query Use_Of_Native_Language
// ----------------------------
// The pure java application is protected from data overflow
// The overflow may appear when native language, like C++ is used
// The purpose of the query to find if the application uses a native language
// The buffer overflow in this situation can break the sandbox security approach

CxList strings = Find_Strings();
CxList methodsLoadLib = All.FindByMemberAccess("*System.loadLibrary");
CxList nativelib = strings.FindByShortName("*native*",false);
result =  methodsLoadLib.FindByParameters(nativelib);