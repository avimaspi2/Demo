CxList allMethods = Find_Methods();
allMethods.Add(Find_MemberAccesses());

List<string> conversionsNames = new List<string>{
		"int","float","long","complex"};

CxList conversions = allMethods.FindByShortNames(conversionsNames);
	
CxList binary = Find_BinaryExpr();

//Remove '+' because of string concatenation
foreach(CxList expr in binary){
	BinaryExpr e = expr.TryGetCSharpGraph<BinaryExpr>();
	if(e.Operator != BinaryOperator.Add)
		result.Add(expr);
}

List<string> operatorsNames = new List<string>{
		"==","!=","<>","<",">","<=",">=","or","and"};

CxList comparisons = binary.FindByShortNames(operatorsNames);
comparisons.Add(Find_Unarys().FindByShortName("Not"));

List<string> sanitizerNames = new List<string>{
		"round","mean","sum","max",
		"min","cos","sin","tan",
		"random","abs","sqrt","tan",
		"*len*","*count*","*getsizeof*","*index*",
		};

CxList numberSanitizer = allMethods.FindByShortNames(sanitizerNames,false);

List<string> sanitizerNamesCaseSensitive = new List<string>{
		"log","strftime","time","datetime"};

numberSanitizer.Add(allMethods.FindByShortNames(sanitizerNamesCaseSensitive));


result = numberSanitizer;
result.Add(conversions);