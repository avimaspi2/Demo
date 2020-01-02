string[] relevantTypes = new string[] {"int", "long", "short", "float", "double", "char", "bool", "size_t"};

CxList types = All.FindByTypes(relevantTypes); 

types.Add(All.FindByReturnType("int"));
types.Add(All.FindByReturnType("long")); 
types.Add(All.FindByReturnType("short")); 
types.Add(All.FindByReturnType("float"));  
types.Add(All.FindByReturnType("double"));  
types.Add(All.FindByReturnType("char"));  
types.Add(All.FindByReturnType("bool"));

CxList typesDef = All.FindDefinition(types);
typesDef -= typesDef.FindByRegex(@"\*");

result = All.FindAllReferences(typesDef);