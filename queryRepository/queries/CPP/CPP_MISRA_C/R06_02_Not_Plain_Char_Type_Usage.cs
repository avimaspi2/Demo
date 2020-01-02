/*
MISRA C RULE 6-2
------------------------------
This query searches non-plain char variables used for storage of non-numeric values

	The Example below shows code with vulnerability:

signed char c = 'x';
unsigned char d = 'y';

*/

// first we build a list of all char typedefs
CxList typedefCharDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers().FindByType("char");
CxList typedefSgnCharDecls = typedefCharDecls.FindByExtendedType("unsigned") 
	+ typedefCharDecls.FindByExtendedType("signed");
ArrayList charTypes = new ArrayList();
ArrayList sgnCharTypes = new ArrayList();
charTypes.Add("char");
charTypes.Add("*.char");
foreach(CxList cur in typedefCharDecls){
	Declarator g = (Declarator) cur.data.GetByIndex(0);
	if (g == null) {
		continue;
	}
	string typeName = g.Name;
	if (!charTypes.Contains(typeName)){
		charTypes.Add(typeName);
		charTypes.Add("*." + typeName);
	}
}
foreach(CxList cur in typedefSgnCharDecls){
	Declarator g = (Declarator) cur.data.GetByIndex(0);
	if (g == null) {
		continue;
	}
	string typeName = g.Name;
	if (!sgnCharTypes.Contains(typeName)){
		sgnCharTypes.Add(typeName);
		sgnCharTypes.Add("*." + typeName);
	}
}

// now build a list of all signed/unsigned char declarators

// first add all declarators of type that is a typedefs of signed/unsigned char
CxList sgnCharDecls = Find_All_Declarators().FindByTypes((string[]) sgnCharTypes.ToArray(typeof(string))) - typedefSgnCharDecls;

// then add all declarators of type unsigned/signed of a typedefed char
CxList charDecls = Find_All_Declarators().FindByTypes((string[]) charTypes.ToArray(typeof(string))) - typedefCharDecls;
sgnCharDecls.Add(charDecls.FindByExtendedType("unsigned") + charDecls.FindByExtendedType("signed"));
sgnCharDecls -= All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();

// now build a list of all plain char instances
CxList unPlainChars = sgnCharDecls + All.FindAllReferences(sgnCharDecls);

CxList stringLiterals = All.FindByType(typeof(StringLiteral));
CxList charLiterals = All.FindByType(typeof(CharLiteral));

result = (charLiterals + stringLiterals).DataInfluencingOn(unPlainChars);