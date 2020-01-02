/// <summary>
/// Find all the references variables declared
/// (as int, long, short, float, double, char, wchar_t and boolean) 
/// </summary>

string[] dataTypes = new string[]{
	"int", "long", "short", "float", "double", "bool"
};

CxList types = All.FindByTypes(dataTypes) - Find_Pointers();
CxList typesDef = All.FindDefinition(types);

result = All.FindAllReferences(typesDef); 
result.Add(Find_Builtin_Char_Types());