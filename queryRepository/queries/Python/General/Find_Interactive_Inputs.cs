CxList methods = Find_Methods();
CxList allParams = Find_MemberAccesses();
CxList paramsAssign = allParams.GetAncOfType(typeof(AssignExpr));
CxList leftAssign = All.GetByAncs(paramsAssign).FindByAssignmentSide(CxList.AssignmentSide.Left);
allParams -= allParams.GetByAncs(leftAssign);

string[] flaskRequest = new string[] {
	"request.files",
	"request.args",
	"request.headers",
	"request.method",
	"request.cookies",
	"request.values",
	"request.stream",
	"request.data",
	"request.environ",
	"request.path",
	"request.full_path",
	"request.script_root",
	"request.base_url",
	"request.get_json",
	"request.url",
	"request.form", 
	"request.args.get"
};

// input from forms
CxList cgi = Find_CGI_Inputs();
CxList django = Find_Django_Inputs();
CxList cherrypy = allParams.FindByName("request.params");
CxList web = methods.FindByName("web.input");
CxList werkzeug = allParams.FindByName("request.form");
CxList flask = Find_Methods_By_Import("flask", flaskRequest);

// Flask @app.route decorator
CxList route = methods.FindByShortName("route");
CxList flk = methods.FindByShortName("Flask").GetAssignee();
CxList app = All.FindAllReferences(flk).GetMembersOfTarget();
CxList fun = route.GetMethod(app);
CxList par = All.GetParameters(fun);
flask.Add(par);

CxList classDec = All.NewCxList();
CxList methodDec = All.NewCxList();
CxList methodRef = All.NewCxList();
CxList funcMethods = All.NewCxList();

methodDec.Add(Find_MethodDecls());
classDec.Add(Find_ClassDecl());
methodRef.Add(All.FindAllReferences(methodDec));

// add_url_rule as in http://flask.pocoo.org/docs/0.12/api/
CxList addUrlRule = methods.FindByShortName("add_url_rule");

// The second parameter is the name of the function we need
CxList secParams = All.GetParameters(addUrlRule, 1);

foreach (CxList p in secParams)
{
	CSharpGraph node = p.GetFirstGraph();
	funcMethods.Add(methodRef.FindByShortName(node.ShortName));
}

flask.Add(All.GetParameters(funcMethods));

// Add inputs from Flask Class Views
CxList refClass = classDec.FindAllReferences(secParams.GetLeftmostTarget());
CxList res = methodDec.FindByShortName("dispatch_request").GetByAncs(refClass);
flask.Add(All.GetParameters(res));

result.Add(cgi);
result.Add(cherrypy);
result.Add(django);
result.Add(web);
result.Add(werkzeug);
result.Add(Find_Cookies());
result.Add(Find_Console_Inputs());
result.Add(Find_HTTPServer_Inputs());
result.Add(flask);