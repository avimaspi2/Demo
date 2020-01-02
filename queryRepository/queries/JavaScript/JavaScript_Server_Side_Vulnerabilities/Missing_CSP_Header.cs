/* This query checks if the project contains CSP (Content Security Policy) at the http headers, and alert if it doesn't.
   If we find at least one place where CSP is used, then no need to alert about it.  */

string csp = "Content-Security-Policy";

//Find use of CSP under Helmet package
List<string> helmetMethods = new List<string>() {"contentSecurityPolicy", "csp"};
CxList helmetRequire = Find_Require("helmet");

CxList helmet = helmetRequire.GetMembersOfTarget().FindByShortNames(helmetMethods);

//Find use of header setting in response
List<string> headerMethodsNames = new List<string>() {"header", "setHeader", "writeHead", "set"};
CxList headers = Find_Methods().FindByShortNames(headerMethodsNames);
CxList cspLiteral = Find_String_Literal().FindByShortName(csp);
CxList cspParameter = cspLiteral.GetParameters(headers);

//For anonymous objects/functions handling 
CxList anonyObjects = Find_LambdaExpr().GetParameters(headers);
CxList anonyClassDecl = Get_Class_Of_Anonymous_Ref(anonyObjects);
CxList anonyMethodDecl = Find_MethodDecls().GetByAncs(anonyClassDecl);
CxList anonyMethodBody = All.GetByAncs(anonyMethodDecl);
foreach (CxList item in anonyMethodBody)
{
	string name = item.GetName();
	if(name != null && name.Equals(csp))
	{
		cspParameter.Add(item);
		break; //If we find 1 - it's enough
	}
}

//No use of CSP detected at all, we should alert about it only once
if(helmet.Count == 0 && cspParameter.Count == 0)
{
	CxList outputs = NodeJS_Find_Interactive_Outputs();
	if(outputs.Count > 0)
	{
		CSharpGraph graph = outputs.GetFirstGraph();
		if(graph != null)
		{
			result = All.FindById(graph.NodeId);
		}
	}
}