//Common functions that do not copy the null terminator.
List<string> methodWithoutNullTermStr = new List<string>{	
		"strncpy","wcsncpy","fread","mbstowcs",	"mbsrtowcs","wcstombs",
		"wcsrtombs","wmemcpy","wmemmove","_memccpy",
		"_strncpy_l","_tcsncpy","_tcsncpy_l","_tcsncat","_tcsncat_l"		
		};

CxList allRelevantTypes = Find_Builtin_Char_Microsoft_Types();
allRelevantTypes.Add(Find_Builtin_Char_Types());
CxList charLiterals = Find_CharLiteral();
CxList allArrayInitializer = Find_ArrayInitializer();
CxList methods = Find_Methods();
CxList unkRefs = Find_Unknown_References();

CxList declWithNullTerm = allArrayInitializer.GetAncOfType(typeof(Declarator));
CxList charToCheckReqNullTerm = unkRefs.GetParameters(methods.FindByShortNames(methodWithoutNullTermStr), 0);

CxList declUnk = Find_Declarators();
declUnk.Add(unkRefs);
allRelevantTypes.Add(declUnk.FindByPointerType("char*"));
allRelevantTypes.Add(declUnk.FindByPointerType("BYTE*"));
allRelevantTypes.Add(declUnk.FindByPointerType("ValueType.Ch"));

CxList relevantParams = unkRefs.FindAllReferences(allRelevantTypes);

List<string> methodsToCheck = new List<string>{"memcpy", "memmove", "fread"};
CxList memCpyParams = relevantParams.GetParameters(methods.FindByShortNames(methodsToCheck), 0);

CxList funcsNames = methods.FindByShortNames(methodsToCheck).GetAncOfType(typeof(MethodDecl));
funcsNames.Add(methods.FindByShortNames(methodWithoutNullTermStr).GetAncOfType(typeof(MethodDecl)));
funcsNames.Add(methods.FindByShortNames(methodWithoutNullTermStr).GetAncOfType(typeof(ConstructorDecl)));

// Sanitizers
/*
	var[4] = '\0';
	var[4] = 0;
 	var[4] = false;
	var[4] = NULL;
*/
CxList sanitizers = All.NewCxList();
IAbstractValue zero = new IntegerIntervalAbstractValue(0);
CxList AssignZero = All.FindByAbstractValue(
	abstractValue => zero.IncludedIn(abstractValue) && !(abstractValue is AnyAbstractValue)
	);
CxList AssignFalse = Find_BooleanLiteral().FindByAbstractValue(abstractValue => abstractValue is FalseAbstractValue);
CxList AssignNull = Find_NullLiteral().FindByAbstractValue(abstractValue => abstractValue is NullAbstractValue);

sanitizers.Add(declWithNullTerm);	
sanitizers.Add(AssignZero);
sanitizers.Add(AssignFalse);
sanitizers.Add(AssignNull);

// use '\0'
foreach(CxList cl in charLiterals){
	string elem = cl.GetName();
	if( !String.IsNullOrEmpty(elem) && elem[0] == '\0'){
		sanitizers.Add(cl);
	}	
}

CxList allRelevantParams = All.NewCxList();
allRelevantParams.Add(charToCheckReqNullTerm);
allRelevantParams.Add(memCpyParams);
CxList allRefsOfRelevantParams = unkRefs.FindAllReferences(allRelevantParams);
sanitizers = allRefsOfRelevantParams.GetByAncs(sanitizers.GetAncOfType(typeof(AssignExpr)));

CxList sanitizerFuncs = All.NewCxList();
sanitizerFuncs.Add(sanitizers.GetAncOfType(typeof(ConstructorDecl)));
sanitizerFuncs.Add(sanitizers.GetAncOfType(typeof(MethodDecl)));

CxList validSanitizerFuncs = (sanitizerFuncs * funcsNames);
sanitizers = unkRefs.FindAllReferences(sanitizers.GetByMethod(validSanitizerFuncs));

// Flow and outputs
result = charToCheckReqNullTerm;
result.Add(memCpyParams);
result -= sanitizers;