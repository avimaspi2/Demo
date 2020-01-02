CxList stringLiteral = Find_String_Literal();
CxList methods = Find_Methods();
CxList objects = Find_ObjectCreations();
CxList methodsAndObjects = methods;
methodsAndObjects.Add(objects);
CxList literalsToRemove = All.NewCxList();
	
// remove parameters of "new Buffer" constructor (usually refers to encoding)
CxList bufferParams = All.GetParameters(methodsAndObjects.FindByShortName("Buffer"), 1)
	.FindByType(typeof(StringLiteral));
literalsToRemove.Add(bufferParams);

//Remove paramters of RegExp invokes
CxList regExpMethods = methods.FindByShortName("RegExp");
CxList regExprParameters = All.GetParameters(regExpMethods);
literalsToRemove.Add(regExprParameters);
//remove all JQueryAccessors
CxList jqueryAccessor = methods.FindByShortName("$");
jqueryAccessor = stringLiteral.GetParameters(jqueryAccessor);
literalsToRemove.Add(jqueryAccessor);

CxList emptyString = stringLiteral.FindByShortName("");
literalsToRemove.Add(emptyString);
stringLiteral -= literalsToRemove;
//Remove hardcoded URLs
stringLiteral -= stringLiteral.FindByShortNames(new List<string>{"https://*", "http://*"});

result = stringLiteral;