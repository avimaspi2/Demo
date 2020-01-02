/*
MISRA C RULE 6-1
------------------------------
This query searches plain char variables used for storage of non character values

	The Example below shows code with vulnerability: 

char c = 5;

*/

// first we build a list of all char typedefs
CxList typedefCharDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers().FindByType("char");
typedefCharDecls -= typedefCharDecls.FindByExtendedType("signed");
typedefCharDecls -= typedefCharDecls.FindByExtendedType("unsigned");

ArrayList charTypes = new ArrayList();
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

// now build a list of all plain char declarators
CxList plainChars = Find_All_Declarators().FindByTypes((string[]) charTypes.ToArray(typeof(string))) - typedefCharDecls;
plainChars -= All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();
plainChars -= plainChars.FindByExtendedType("signed");
plainChars -= plainChars.FindByExtendedType("unsigned");


//find pointer declarations and remove them from char list
CxList pointers = plainChars.FindByRegex(@"(?:\*\s*)\w", false, false, false);
plainChars -= pointers;
plainChars.Add(All.FindAllReferences(plainChars));

CxList integerLiterals = All.FindByType(typeof(IntegerLiteral));

/* remove array size literals */
integerLiterals -= integerLiterals.GetByAncs(integerLiterals.GetAncOfType(typeof(ArrayCreateExpr)));
integerLiterals -= integerLiterals.FindByRegex(@"\[\w+?\]",false,false,false);

result = integerLiterals.DataInfluencingOn(plainChars);