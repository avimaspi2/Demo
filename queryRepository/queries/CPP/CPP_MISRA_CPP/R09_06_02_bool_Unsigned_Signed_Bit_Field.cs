/*
MISRA CPP RULE 9-6-2
------------------------------
This query searches for bit fields of type other than  bool and signed/unsigned integral type.
Bit fields of type wchar_t are also non compliant.

	The Example below shows code with vulnerability: 

		struct a
    	{
             int a:1; //non-compliant
			 double c:2; //non compliant;
             wchar_t w:1; //non-compliant;

        }

*/

// first we build a list of all int typedefs

CxList temp = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();
CxList typedefIntDecls = temp.FindByType("int") + temp.FindByType("char") + temp.FindByType("short") + temp.FindByType("long");
CxList typedefSgnIntDecls = typedefIntDecls.FindByExtendedType("unsigned") 
	+ typedefIntDecls.FindByExtendedType("signed");
ArrayList intTypes = new ArrayList();
ArrayList sgnIntTypes = new ArrayList();
//intTypes.Add("");
intTypes.Add("*.");
intTypes.Add("int");
intTypes.Add("*.int");
intTypes.Add("char");
intTypes.Add("*.char");
intTypes.Add("short");
intTypes.Add("*.short");
intTypes.Add("long");
intTypes.Add("*.long");
foreach(CxList cur in typedefIntDecls){
	string typeName = ((Declarator) cur.data.GetByIndex(0)).Name;
	if(typeName != null)
	{
		if (!intTypes.Contains(typeName))
		{
			intTypes.Add(typeName);
			intTypes.Add("*." + typeName);
		}
	}
}
foreach(CxList cur in typedefSgnIntDecls){
	string typeName = ((Declarator) cur.data.GetByIndex(0)).Name;
	if (typeName!=null && !sgnIntTypes.Contains(typeName)){
		sgnIntTypes.Add(typeName);
		sgnIntTypes.Add("*." + typeName);
	}
}

// now build a list of all signed/unsigned int declarators

CxList dcltr = Find_All_Declarators();
// first add all declarators of type that is a typedefs of signed/unsigned int
CxList sgnIntDecls = dcltr.FindByTypes((string[]) sgnIntTypes.ToArray(typeof(string))) - typedefSgnIntDecls;

// then add all declarators of type unsigned/signed of a typedefed int
CxList intDecls = dcltr.FindByTypes((string[]) intTypes.ToArray(typeof(string))) - typedefIntDecls;

sgnIntDecls.Add(intDecls.FindByExtendedType("unsigned") + intDecls.FindByExtendedType("signed"));

sgnIntDecls.Add(dcltr.FindByType("bool"));

// find all potential non signed/unsigned int bitfields
CxList potentialBitFields = dcltr
	+ All.FindByType(typeof(EnumMemberDecl)).GetFathers().GetFathers();
potentialBitFields -= typedefIntDecls;
potentialBitFields -= sgnIntDecls;

result = potentialBitFields.FindByRegex(@"[\}|\w]+?\s*?:\s*?\d+?\s*?;", false, false,false);