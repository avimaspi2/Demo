// Bit_Fields_Length
// Purpose:
// find bit fields of type signed int of length < 2 bits
// ===============================

// first we build a list of all int typedefs
CxList typedefIntDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers().FindByType("int");
CxList typedefSgnIntDecls = typedefIntDecls.FindByExtendedType("signed");
ArrayList intTypes = new ArrayList();
ArrayList sgnIntTypes = new ArrayList();
//intTypes.Add("");
intTypes.Add("*.");
intTypes.Add("int");
intTypes.Add("*.int");
foreach(CxList cur in typedefIntDecls){
	string typeName = ((Declarator) cur.data.GetByIndex(0)).Name;
	if (!intTypes.Contains(typeName)){
		intTypes.Add(typeName);
		intTypes.Add("*." + typeName);
	}
}
foreach(CxList cur in typedefSgnIntDecls){
	string typeName = ((Declarator) cur.data.GetByIndex(0)).Name;
	if (!sgnIntTypes.Contains(typeName)){
		sgnIntTypes.Add(typeName);
		sgnIntTypes.Add("*." + typeName);
	}
}

// now build a list of all signed int declarators

// first get add all declarators of type that is a typedefs of signed int
CxList sgnIntDecls = Find_All_Declarators().FindByTypes((string[]) sgnIntTypes.ToArray(typeof(string))) - typedefSgnIntDecls;

// then add all declarators of type signed of a typedefed int
CxList intDecls = Find_All_Declarators().FindByTypes((string[]) intTypes.ToArray(typeof(string))) - typedefIntDecls;
sgnIntDecls.Add(intDecls.FindByExtendedType("signed"));

// return the signed int bit fields of length < 2
result = sgnIntDecls.FindByRegex(@"[\}|\w]+?\s*?:\s*?[0|1]\s*?;",false,false,false);