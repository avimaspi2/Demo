/*
MISRA CPP RULE 2-13-3
------------------------------
This query searches for unsigned constants without the 'U' suffix

	The Example below shows code with vulnerability: 

use_uint64 ( 9223372036854775808 );
use_uint32 ( 0x80000000 );

*/

// System definitions
const int INT_BITS = 16;
const int LONG_BITS = 32;

// The limits on range of int and long, based on system definition
ulong MAX_SIGNED_INT = (ulong) Math.Pow(2, INT_BITS - 1) - 1;
ulong MAX_UNSIGNED_INT = (ulong) Math.Pow(2, INT_BITS) - 1;
ulong MAX_SIGNED_LONG = (ulong) Math.Pow(2, LONG_BITS - 1) - 1;
ulong MAX_UNSIGNED_LONG = (ulong) Math.Pow(2, LONG_BITS) - 1;

//
// first we build a list of all unsigned typedefs
CxList typedefDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();
CxList typedefUSgnDecls = typedefDecls.FindByExtendedType("unsigned");
ArrayList usgnTypes = new ArrayList();
foreach(CxList cur in typedefUSgnDecls){
	Declarator g = (Declarator) cur.data.GetByIndex(0);
	if (g == null || g.Name == null) {
		continue;
	}
	string typeName = g.Name;
	if (!usgnTypes.Contains(typeName)){
		usgnTypes.Add(typeName);
		usgnTypes.Add("*." + typeName);
	}
}

// Find all declarations: varialbe, parameter and field.
CxList declarators = Find_All_Declarators() + All.FindByType(typeof(ParamDecl)) + All.FindByType(typeof(FieldDecl));
// first get add all declarators of type that is a typedefs of signed
CxList usgnDecls = declarators.FindByTypes((string[]) usgnTypes.ToArray(typeof(string)));
// then add all declarators of a signed type
usgnDecls.Add(declarators.FindByExtendedType("unsigned"));
usgnDecls -= typedefDecls;

//
//
// Find variables and parameters defined as unsigned numbers

CxList intDeclarators = All.FindByType("int");
CxList unknownRef = All.FindByType(typeof(UnknownReference));

CxList unsignedRefs = intDeclarators.FindByExtendedType("unsigned");
unsignedRefs = All.FindAllReferences(unsignedRefs);

CxList stdintDeclarators = usgnDecls;

unsignedRefs = (unsignedRefs + unknownRef).FindAllReferences(unsignedRefs + stdintDeclarators);

// Get left side of assignment with unsigned variable
CxList leftSide = unsignedRefs.FindByAssignmentSide(CxList.AssignmentSide.Left) + stdintDeclarators;

CxList fathers = leftSide.GetFathers();

CxList allChildren = All.GetByAncs(fathers);
// Elements from right side of assignment
CxList rightSide = allChildren - leftSide;

// Remove from right side method invokes sub tree.
CxList methodInvokes = rightSide.FindByType(typeof(MethodInvokeExpr));
rightSide -= rightSide.GetByAncs(methodInvokes);

CxList temp;
CxList integerLiterals = All.FindByType(typeof(IntegerLiteral));

// octal literals with no 'U' or 'u' suffix
CxList octalLiterals = All.NewCxList();
temp = integerLiterals.FindByRegex(@"[^\w]0[0-7]*(L|l)?[^\wuU]", false, false, false, octalLiterals);

// hexadecimal literals with no 'U' or 'u' suffix
CxList hexaLiterals = All.NewCxList();
temp = integerLiterals.FindByType(typeof(IntegerLiteral)).FindByRegex(@"[^\w]0[x|X][0-9,a-fA-F]+(L|l)?[^G-Za-z0-9_uU]", false, false, false, hexaLiterals);

hexaLiterals.Add(octalLiterals);

CxList resultsText = All.NewCxList();

// go over the various found numeric literals, check if the underlying type is unsigned.

// add octals/hexas
foreach (CxList cur in (hexaLiterals)){
	try
	{
		string str = (cur.data.GetByIndex(0) as CSharpGraph).FullName;

		str = str.Substring(3, str.Length - 4);

		char lastChar = str[str.Length - 1];
		if ((lastChar == 'L') || (lastChar == 'l')){
			str = str.Substring(0, str.Length - 1);
		}		
		long numberLong = Int64.Parse(str, System.Globalization.NumberStyles.HexNumber);

		if (numberLong < 0){
			resultsText.Add(cur);
			continue;
		}
	
		if (!((lastChar == 'L') || (lastChar == 'l'))){
			ulong number = ((ulong) numberLong);
		
			if ((number > MAX_SIGNED_INT) && (number <= MAX_SIGNED_LONG)){
				resultsText.Add(cur);
				continue;
			}			
		}
	}
	catch (Exception e){}
}

// find all variable declaration statements of unsigned types.
CxList varDeclStmt = leftSide.GetAncOfType(typeof(VariableDeclStmt));

CxList suspectedNumbers = All.FindByRegexSecondOrder(".", resultsText);
result = suspectedNumbers.GetByAncs(varDeclStmt);
methodInvokes = All.FindByType(typeof(MethodInvokeExpr)).GetByAncs(result);

result.Add(suspectedNumbers * rightSide);

// Remove from right side method invokes sub tree.
result -= result.GetByAncs(methodInvokes);

//
// extract parameters of method invokes.
//
CxList functionOfSuspected = suspectedNumbers.FindByType(typeof(Param)).GetAncOfType(typeof(MethodInvokeExpr)).FindByFileName(@"*Misra_2-13-3_example02.cpp");
CxList methodDecl = All.FindByType(typeof(MethodDecl));
CxList paramDeclCol = All.FindByType(typeof(ParamDeclCollection));
CxList paramDecl = All.FindByType(typeof(ParamDecl));

foreach(CxList func in functionOfSuspected)
{
	CxList parameters = All.FindByType(typeof(Param)).GetParameters(func);
	int number = parameters.Count;
	// find method definition
	CxList methodDeclaration = methodDecl.FindDefinition(func);
	// locate position of unsigned paramters and validate value it gets.
	CxList collection = paramDeclCol.FindByFathers(methodDeclaration);	
	for (int i = 0; i < number ; i++)
	{	
		CxList methodParam = suspectedNumbers.GetParameters(func, i);		

		if(methodParam.Count == 1)
		{
			CxList toCheck = paramDecl.GetParameters(methodDeclaration, i);
			toCheck.Add(unsignedRefs.GetByAncs(toCheck.GetAncOfType(typeof(ParamDecl))));
			if((toCheck * unsignedRefs).Count > 0)
			{
				result.Add(methodParam);
			}
		}		
	}
}