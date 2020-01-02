/*
	Return inputs for classes that inherits from BaseHTTPRequestHandler, CGIHTTPRequestHandler and SimpleHTTPRequestHandler 
	1. Find all objects in classes that inherits form HTTP 3 clases
	2. Find all this objects of classes that inherits from 3 HTTP classes, 
	   this objects are parameter and it's references of do_HEAD, do_GET and do_POST methdods
	3. Find logs that are methods or properties of "2"
*/
//1. Find all objects in classes that inherits form HTTP 3 clases
CxList allClasses = Find_ClassDecl();

CxList inheritants = allClasses.InheritsFrom("BaseHTTPRequestHandler");
inheritants.Add(allClasses.InheritsFrom("CGIHTTPRequestHandler"));
inheritants.Add(allClasses.InheritsFrom("SimpleHTTPRequestHandler"));

CxList allInInheritClasses = All.GetByAncs(inheritants);

//2. Find all this objects of classes that inherits from 3 HTTP classes
CxList thisOfInherits = allInInheritClasses.FindByType(typeof (ThisRef));
CxList membersOfThis = thisOfInherits.GetMembersOfTarget();

//3.Find inputs that are methods or properties of "2"
CxList logErrOrMess = membersOfThis.FindByShortName("log_error");
logErrOrMess.Add(membersOfThis.FindByShortName("log_message"));

//only second parameter of send_error
CxList sendError = membersOfThis.FindByShortName("send_error");
sendError = allInInheritClasses.GetParameters(sendError, 1);
CxList removed = allInInheritClasses.FindByType(typeof (BinaryExpr));
removed.Add(allInInheritClasses.FindByType(typeof(Param)));
removed.Add(allInInheritClasses.FindByType(typeof(StringLiteral)));
removed.Add(allInInheritClasses.FindByType(typeof(IntegerLiteral)));
CxList relativeParams = allInInheritClasses - removed;
sendError.Add(relativeParams.GetByAncs(sendError));
sendError -= removed;

result = logErrOrMess;
result.Add(sendError);