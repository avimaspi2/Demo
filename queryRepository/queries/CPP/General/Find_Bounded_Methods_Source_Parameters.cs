/* 
This query should retrieve all methods that  do some kind of manipulation 
from a source to a destination based on the source to avoid false positives in data without encryption
Ex: 
strncpy ( str2, str1, sizeof(password) ); <- False positive , this should´t flag sizeof(password)
   
lstrcpyn( passwordCopy, password, sizeof(password)); <- True positive , this should flag since we are copy
the value of password to passwordCopy 

*/

List<string> copyMethods = new List<string> { "memcpy", "wmemcpy", "memmove", "wmemmove",
		"strncpy", "_strncpy*", "lstrcpyn","_tcsncpy*", "_mbsnbcpy*", "wcsncpy",
		"strncat", "_strncat*", "_mbsncat*", "wcsncat*" };

CxList methods = Find_Methods();
result = All.GetParameters(methods.FindByShortNames(copyMethods),1);