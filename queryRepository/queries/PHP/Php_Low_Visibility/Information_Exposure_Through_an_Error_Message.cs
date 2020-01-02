CxList ctch = All.FindByType(typeof(Catch));
CxList outputs = Find_Interactive_Outputs();

CxList exc = All.FindAllReferences(ctch) - ctch;

CxList methods = Find_Methods();

CxList errOuts = methods.FindByShortNames(new List<String>(){"error_reporting", "phpinfo"});
//exclude those that are error_reporting(0);
CxList ints = All.FindByType(typeof(IntegerLiteral));
CxList zeroSource = ints.FindByShortName("0");
CxList zeroSink = errOuts.FindByParameters(zeroSource);
errOuts -= zeroSink;
// Find cases such as ini_set('error_reporting', [PARAM]);
CxList setters = methods.FindByShortName("ini_set");
CxList source = Find_Strings().FindByShortName("error_reporting");
CxList errSetters = methods.FindByParameters(source.GetParameters(setters, 0));
//exclude those that are init_set('error_reporting, 0);
CxList excludeMethods = methods.FindByParameters(zeroSource.GetParameters(setters, 1));
errSetters -= excludeMethods;

errOuts += errSetters;
result = outputs.DataInfluencedBy(exc) + errOuts;