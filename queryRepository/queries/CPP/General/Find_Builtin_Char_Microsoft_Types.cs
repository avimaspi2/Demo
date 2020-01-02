/// <summary>
/// Find all the char references variables declared
/// (BYTE, TBYTE, CCHAR, UCHAR) 
/// </summary>
///

string[] microsoftTypes = new string[]{
    "BYTE", "CCHAR", "CHAR", "LPBYTE", "LPCSTR",
	"LPCTSTR", "LPCWSTR", "LPSTR", "LPTSTR", "LPWSTR", "PBYTE", "PCHAR",
	"PCSTR", "PCTSTR", "PCWSTR", "PSTR", "PTBYTE", "PTCHAR", "PTSTR", 
	"PUCHAR", "PWCHAR", "PWSTR", "TBYTE", "TCHAR", "UCHAR", "WCHAR"
	};

CxList types = All.FindByTypes(microsoftTypes) - Find_Pointers();
CxList typesDef = All.FindDefinition(types);

result = All.FindAllReferences(typesDef);