/*
	Return inputs for classes that inherits from BaseHTTPRequestHandler, CGIHTTPRequestHandler and SimpleHTTPRequestHandler 
	1. Find all objects in classes that inherits form HTTP 3 clases
	2. Find all this objects of classes that inherits from 3 HTTP classes, 
	   this objects are parameter and it's references of do_HEAD, do_GET and do_POST methdods
	3. Find inputs that are methods or properties of "2"
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
CxList paths = membersOfThis.FindByShortName("path");

CxList headers = membersOfThis.FindByShortName("headers");
headers = headers.GetMembersOfTarget();

CxList rFile = membersOfThis.FindByShortName("rfile");
rFile = rFile.GetMembersOfTarget();
CxList reads = rFile.FindByShortName("read");
reads.Add(rFile.FindByShortName("readline"));
reads.Add(rFile.FindByShortName("readlines"));
reads.Add(rFile.FindByShortName("readall"));

result = paths;
result.Add(headers);
result.Add(reads);