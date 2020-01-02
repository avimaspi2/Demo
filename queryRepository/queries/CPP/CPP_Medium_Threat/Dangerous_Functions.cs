CxList methods = Find_Methods();
CxList _methods = methods.FindByShortName("_*");
CxList Chamethods = methods.FindByShortName("Cha*");
CxList IsBadmethods = methods.FindByShortName("IsBad*");
CxList lstrmethods = methods.FindByShortName("lstr*");
CxList strmethods = methods.FindByShortName("str*", false);
CxList wmethods = methods.FindByShortName("w*");

// Dangerous functions
CxList dangerous = 
	//	methods.FindByShortName("fgets") +
	methods.FindByShortNames(new List<string>{"atoi","mbscpy", "olestrcpy", "realloc", "ualstrcpy",
		"ualstrcpyA", "ualstrcpyW", "vfork", "MultiByteToWideChar"}, false) + 
	strmethods.FindByShortNames(new List<string>{"StrCAdd", "StrCCpy", "StrCpyA"}, false);

	
	// Micosoft SDLC security banned methods ()
List<string> _methodNameList = new List<string>{"_alloca","_ftcscat", "_ftcscpy", "_getts", "_gettws","_i64toa",
		"_i64tow","_itoa","_itow","_makepath","_mbccat","_mbccpy","_mbscat","_mbscpy","_mbslen","_mbsnbcat",
		"_mbsnbcpy","_mbsncat","_mbsncpy","_mbstok","_mbstrlen","_snprintf","_sntprintf","_sntscanf","_snwprintf",
		"_splitpath","_stprintf","_stscanf","_tccat","_tccpy","_tcscat","_tcscpy","_tcslen","_tcsncat","_tcsncpy",
		"_tcstok","_tmakepath","_tscanf","_tsplitpath","_ui64toa","_ui64tot","_ui64tow","_ultoa","_ultot","_ultow",
		"_vsnprintf","_vsntprintf","_vsnwprintf","_vstprintf","_wmakepath","_wsplitpath"};

List<string> chaMethodsList = new List<string>{"ChangeWindowMessageFilter","CharToOem","CharToOemA","CharToOemBuffA","CharToOemBuffW","CharToOemW"};

List<string> IsBadMethodsList = new List<string>{"IsBadCodePtr","IsBadHugeReadPtr","IsBadHugeWritePtr","IsBadReadPtr","IsBadStringPtr","IsBadWritePtr"};

List<string> lstrMethodsList = new List<string>{"lstrcat","lstrcatA","lstrcatn","lstrcatnA","lstrcatnW","lstrcatW","lstrcpy","lstrcpyA","lstrcpyn",
		"lstrcpynA","lstrcpynW","lstrcpyW","lstrlen","lstrncat"};

List<string> methodsNameList = new List<string>{"makepath","memcpy","OemToChar","OemToCharA","OemToCharW","RtlCopyMemory","scanf","snscanf","snwscanf",
		"sprintf","sprintfA","sprintfW","sscanf","swprintf","swscanf","vsnprintf","vsprintf","vswprintf","alloca","CopyMemory",	"gets"};

List<string> strMethodsList = new List<string>{"strcat","StrCat","strcatA","StrCatA","StrCatBuff","StrCatBuffA","StrCatBuffW","StrCatChainW","StrCatN",
		"StrCatNA",	"StrCatNW","strcatW","StrCatW","strcpy","StrCpy","strcpyA","StrCpyA","StrCpyN","StrCpyNA","strcpynA","StrCpyNA","strcpynA",
		"StrCpyNW",	"strcpyW","StrCpyW","strlen","StrLen","strlen","StrLen","strncat","StrNCat","StrNCatA","StrNCatW","strncpy","StrNCpy",
		"strncpy","StrNCpy","StrNCpyA","StrNCpyW","strtok"};

List<string> wMethodsList = new List<string>{	"wcscat","wcscpy","wcslen","wcsncat","wcsncpy","wcstok","wmemcpy","wnsprintf","wnsprintfA",	"wnsprintfW",
		"wscanf","wsprintf","wsprintfA","wsprintfW","wvnsprintf","wvnsprintfA","wvnsprintfW","wvsprintf","wvsprintfA","wvsprintfW"};	
		
CxList microsoft = _methods.FindByShortNames(_methodNameList);
microsoft.Add(Chamethods.FindByShortNames(chaMethodsList));
microsoft.Add(IsBadmethods.FindByShortNames(IsBadMethodsList));
microsoft.Add(lstrmethods.FindByShortNames(lstrMethodsList));
microsoft.Add(methods.FindByShortNames(methodsNameList));
microsoft.Add(strmethods.FindByShortNames(strMethodsList));
microsoft.Add(wmethods.FindByShortNames(wMethodsList));

/*use '\0' as sanitizer to strncpy*/
CxList myAssign = All.FindByType(typeof(AssignExpr));
CxList charList = All.FindByType(typeof(CharLiteral));
CxList aux = All.NewCxList();
foreach(CxList cl in charList){
	if((cl.TryGetCSharpGraph<CharLiteral>().ShortName)[0] == '\0'){
		aux.Add(cl);
	}	
}
CxList myArrays = All.FindAllReferences(aux.GetAssignee().FindByType(typeof(IndexerRef)));
CxList strcpyList = methods.FindByShortName("strncpy");
CxList aux1 = myArrays.GetParameters(strcpyList, 0).GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("strncpy");
microsoft -= aux1;

result = microsoft + dangerous;