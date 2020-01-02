CxList res = All.FindByShortNames(new List<string>{"res", "response"});
CxList uRef = Find_UnknownReference();
List<string> names = new List<string>(new string[]{"redirect", "send", "cookie", "locals", "param", "params", "setHeader",
	"download", "location", "sendFile", "write", "writeHead", "statusMessage", "addTrailers", "render" });
CxList temp = res.Clone();
CxList found;
CxList post = All.NewCxList();

for(int i = 0; i < 10 && temp.Count > 0; i++)
{              
	temp = temp.GetMembersOfTarget();
	found = temp.FindByShortNames(names);
	post.Add(found);
	temp -= found;
}

// Add response mapped by Express: 'res' in app.get(path , function(req, res...) {})'
CxList expressCallbak = NodeJS_Find_Express_Callback_Params(res);
post.Add(uRef.FindAllReferences(expressCallbak));
CxList potential = All.NewCxList();
CxList pMembers = All.NewCxList();
foreach(CxList p in post)
{
	potential = p.Clone();
	for(int j = 0; j < 10 && potential.Count > 0; j++)
	{
		pMembers = potential.GetMembersOfTarget();
		if(pMembers.Count == 0)
		{
			if (( potential.FindByType(typeof(MethodInvokeExpr)).Count > 0) || (potential.FindByAssignmentSide(CxList.AssignmentSide.Left).Count > 0) )
			{
				result.Add(potential);	// only if the member is a method or a setter - add it
			}
			break;
		}
		potential = pMembers;
	}
}
result = result.FindByShortName("redirect");
if(Hapi_Find_Server_Instance().Count > 0)
{
	result.Add(Hapi_Find_Outputs_Redirect());
}

result -= XS_Find_All();