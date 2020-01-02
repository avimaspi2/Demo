/// <summary>
/// Find all the char references variables declared
/// (char and wchar_t) 
/// </summary>
///

string[] charTypes = new string[]{
	"char", "wchar_t"
};

CxList types = All.FindByTypes(charTypes) - Find_Pointers();
CxList typesDef = All.FindDefinition(types);

result = All.FindAllReferences(typesDef);